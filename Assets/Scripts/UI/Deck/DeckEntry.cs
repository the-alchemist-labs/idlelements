using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEntry : MonoBehaviour
{
    [SerializeField]
    Image elementalImage;
    [SerializeField]
    Image caughtIndicatorImage;
    [SerializeField]
    TMP_Text elementalIdText;
    [SerializeField]
    TMP_Text elementalName;
    [SerializeField]
    TMP_Text tokensText;
    [SerializeField]
    TMP_Text idleBonusText;
    [SerializeField]
    Button evolveButtonInfo;

    private Elemental _elemental;
    private EvolvePanel _evolvePanel;

    private Color _unlockedColor = Color.white;
    private Color _lockedColor = new Color(.25f, .25f, .25f, 0.8f);

    void Start()
    {
        _lockedColor.a = 0.5f;
    }

    public void UpdateEntry(Elemental elemental, EvolvePanel evolvePanel)
    {
        _elemental = elemental;
        _evolvePanel = evolvePanel;

        ElementalEntry entry = ElementalManager.Instance.GetElementalEntry(elemental.id);
        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        elementalIdText.text = $"#{(int)elemental.id}";
        elementalName.text = elemental.name;
        tokensText.text = $"Tokens: {entry.tokens}";
        evolveButtonInfo.gameObject.SetActive(elemental.evolution != null);
        evolveButtonInfo.GetComponent<Image>().color = ElementalManager.Instance.CanEvolve(elemental.id) ? Color.white : Color.gray;

        UpdateCatchImage();
        UpdateIdleBonusInfo();
    }

    public void EvolveInfoClicked()
    {
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);
        _evolvePanel.DisplayPanel(_elemental);
    }

    private void UpdateCatchImage()
    {
        if (ElementalManager.Instance.GetElementalEntry(_elemental.id).isCaught)
        {
            elementalImage.color = _unlockedColor;
            caughtIndicatorImage.gameObject.SetActive(true);
        }
        else
        {
            elementalImage.color = _lockedColor;
            caughtIndicatorImage.gameObject.SetActive(false);
        }
    }

    private void UpdateIdleBonusInfo()
    {
        if (_elemental.idleBonus != null)
        {
            idleBonusText.text = $"Idle bonus: {_elemental.idleBonus.amount * 100}% {_elemental.idleBonus.resource}";
        }
    }
}