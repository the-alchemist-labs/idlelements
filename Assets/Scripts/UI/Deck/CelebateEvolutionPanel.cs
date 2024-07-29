using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CelebateEvolutionPanel : MonoBehaviour
{
    [SerializeField]
    TMP_Text evolveToText;
    [SerializeField]
    TMP_Text idleBonusText;
    [SerializeField]
    Image sprite;

    public void DisplayPanel(Elemental elemental)
    {
        gameObject.SetActive(true);

        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Celebration);

        Elemental evolution = ElementalCatalog.Instance.GetElemental(elemental.evolution.evolveTo);
        IdleBonus idleBonus = evolution.idleBonus;

        sprite.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{evolution.id}");
        evolveToText.text = $"Your {elemental.name} evolved to {evolution.name}";
        idleBonusText.text = idleBonus != null ? $"Idle bonus: +{idleBonus.amount * 100} {idleBonus.resource}" : "";

    }
}
