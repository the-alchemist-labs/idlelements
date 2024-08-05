using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CelebateEvolutionPanel : MonoBehaviour
{
    [SerializeField]
    TMP_Text evolveToText;
    [SerializeField]
    Image sprite;

    public void DisplayPanel(Elemental elemental)
    {
        gameObject.SetActive(true);

        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Celebration);

        Elemental evolution = ElementalCatalog.Instance.GetElemental(elemental.Evolution.evolveTo);

        sprite.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{evolution.Id}");
        evolveToText.text = $"Your {elemental.name} evolved to {evolution.name}";
    }
}
