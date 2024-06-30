using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEntry : MonoBehaviour
{
    public Image elementalImage;
    public Image caughtIndicatorImage;
    public TMP_Text elementalIdText;
    public TMP_Text elementalName;
    public TMP_Text tokensText;
    public TMP_Text idleBonusText;
    public TMP_Text evolveToText;
    public TMP_Text essenceEvolveCost;
    public TMP_Text tokensEvolveCost;
    public Button evolveButton;

    private Elemental elemental;

    private Color UnlockedColor = Color.white;
    private Color LockedColor = new Color(.25f, .25f, .25f, 0.8f);

    void Start()
    {
        LockedColor.a = 0.5f;
    }

    public void UpdateEntry(Elemental elemental)
    {
        this.elemental = elemental;
        ElementalEntry entry = State.Elementals.GetElementalEntry(elemental.id);
        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        elementalIdText.text = $"#{(int)elemental.id}";
        elementalName.text = elemental.name;
        tokensText.text = $"Tokens: {entry.tokens}";

        UpdateCatchImage();
        UpdateIdleBonusInfo();
        UpdateEvolveInfo();
    }

    public void Evolve()
    {
        State.Elementals.Evolve(elemental.id);
    }

    void UpdateCatchImage()
    {
        if (State.Elementals.GetElementalEntry(elemental.id).isCaught)
        {
            elementalImage.color = UnlockedColor;
            caughtIndicatorImage.gameObject.SetActive(true);
        }
        else
        {
            elementalImage.color = LockedColor;
            caughtIndicatorImage.gameObject.SetActive(false);
        }
    }

    void UpdateEvolveInfo()
    {
        if (elemental.evolution != null)
        {
            evolveToText.text = $"To {elemental.evolution.evolveTo}";
            essenceEvolveCost.text = TextUtil.NumberFormatter(elemental.evolution.essenceCost);
            tokensEvolveCost.text = TextUtil.NumberFormatter(elemental.evolution.tokensCost);
            evolveButton.interactable = State.Elementals.CanEvolve(elemental.id);
            evolveButton.gameObject.SetActive(true);
        }
        else
        {
            evolveToText.text = "None";
            evolveButton.gameObject.SetActive(false);
        }

    }

    void UpdateIdleBonusInfo()
    {
        if (elemental.idleBonus != null)
        {
            idleBonusText.text = $"Idle bonus: {elemental.idleBonus.amount * 100}% {elemental.idleBonus.resource}";
        }
    }
}