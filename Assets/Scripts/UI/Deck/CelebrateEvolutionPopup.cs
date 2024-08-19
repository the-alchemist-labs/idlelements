using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CelebrateEvolutionPopup : BasePopup
{
    public override PopupId Id { get; } = PopupId.CelebrateEvolution;
    
    [SerializeField] TMP_Text evolveToText;
    [SerializeField] Image sprite;
    
    public void DisplayPanel(Elemental elemental)
    {
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Celebration);

        Elemental evolution = ElementalCatalog.Instance.GetElemental(elemental.Evolution.evolveTo);

        sprite.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{evolution.Id}");
        evolveToText.text = $"Your {elemental.name} evolved to {evolution.name}";
    }
}