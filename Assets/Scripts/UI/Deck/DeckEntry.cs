using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEntry : MonoBehaviour
{
    [SerializeField] Image elementalImage;
    [SerializeField] Image caughtIndicatorImage;
    [SerializeField] TMP_Text elementalIdText;
    [SerializeField] TMP_Text elementalName;
    [SerializeField] TMP_Text tokensText;
    [SerializeField] Button evolveButtonInfo;

    private Elemental _elemental;

    private Color _unlockedColor = Color.white;
    private Color _lockedColor = new Color(.25f, .25f, .25f, 0.8f);

    void Start()
    {
        _lockedColor.a = 0.5f;
    }

    public void UpdateEntry(Elemental elemental)
    {
        _elemental = elemental;
        ElementalEntry entry = ElementalManager.Instance.GetElementalEntry(elemental.Id);
        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.Id}");
        elementalIdText.text = $"#{(int)elemental.Id}";
        elementalName.text = elemental.name;
        tokensText.text = $"Tokens: {entry.tokens}";
        evolveButtonInfo.gameObject.SetActive(elemental.Evolution.evolveTo != ElementalId.None);
        evolveButtonInfo.GetComponent<Button>().interactable = ElementalManager.Instance.CanEvolve(elemental.Id);
    
        UpdateCatchImage();
    }

    public void EvolveInfoClicked()
    {
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);
        EvolvePopup popup = PopupManager.Instance.OpenPopUp<EvolvePopup>(PopupId.Evolve);
        popup.DisplayPanel(_elemental);
    }

    private void UpdateCatchImage()
    {
        if (ElementalManager.Instance.GetElementalEntry(_elemental.Id).isCaught)
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
}