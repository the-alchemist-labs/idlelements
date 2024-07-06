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
    public Button evolveButtonInfo;

    private Elemental elemental;
    private EvolvePanel evolvePanel;

    private Color UnlockedColor = Color.white;
    private Color LockedColor = new Color(.25f, .25f, .25f, 0.8f);

    void Start()
    {
        LockedColor.a = 0.5f;
    }

    public void UpdateEntry(Elemental elemental, EvolvePanel evolvePanel)
    {
        this.elemental = elemental;
        this.evolvePanel = evolvePanel;

        ElementalEntry entry = State.Elementals.GetElementalalEntry(elemental.id);
        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        elementalIdText.text = $"#{(int)elemental.id}";
        elementalName.text = elemental.name;
        tokensText.text = $"Tokens: {entry.tokens}";
        evolveButtonInfo.gameObject.SetActive(elemental.evolution != null);
        evolveButtonInfo.GetComponent<Image>().color = State.Elementals.CanEvolve(elemental.id) ? Color.white : Color.gray;

        UpdateCatchImage();
        UpdateIdleBonusInfo();
    }

    public void EvolveInfoClicked()
    {
        evolvePanel.DisplayPanel(elemental);
    }

    void UpdateCatchImage()
    {
        if (State.Elementals.GetElementalalEntry(elemental.id).isCaught)
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

    void UpdateIdleBonusInfo()
    {
        if (elemental.idleBonus != null)
        {
            idleBonusText.text = $"Idle bonus: {elemental.idleBonus.amount * 100}% {elemental.idleBonus.resource}";
        }
    }
}