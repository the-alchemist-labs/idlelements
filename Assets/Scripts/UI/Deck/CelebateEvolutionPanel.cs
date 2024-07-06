using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CelebateEvolutionPanel : MonoBehaviour
{
    public TMP_Text evolveToText;
    public TMP_Text idleBonusText;
    public Image sprite;
    public AudioSource celebrationSound;

    public void DisplayPanel(Elemental elemental)
    {
        gameObject.SetActive(true);

        celebrationSound.Play();
        
        Elemental evolution = State.Elementals.GetElemental(elemental.evolution.evolveTo);
        IdleBonus idleBonus = evolution.idleBonus;

        sprite.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{evolution.id}");
        evolveToText.text = $"Your {elemental.name} evolved to {evolution.name}";
        idleBonusText.text = idleBonus != null ? $"Idle bonus: +{idleBonus.amount * 100} {idleBonus.resource}" : "";

    }
}
