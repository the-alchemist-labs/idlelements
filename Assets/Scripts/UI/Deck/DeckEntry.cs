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

    private ElementalId elementalId;

    private Color UnlockedColor = Color.white;
    private Color LockedColor = new Color(.25f, .25f, .25f, 0.8f);

    void Start()
    {
        LockedColor.a = 0.5f;
        GameEvents.OnElementalCaught += UpdateCatchImage;
    }

    void OnDestroy()
    {
        GameEvents.OnElementalCaught -= UpdateCatchImage;
    }

    public void UpdateEntry(Elemental elemental)
    {
        elementalId = elemental.id;
        ElementalEntry entry = State.Elementals.GetElementalEntry(elemental.id);
        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        elementalIdText.text = $"#{(int)elemental.id}";
        elementalName.text = elemental.name;
        tokensText.text = $"Tokens: {entry.tokens}";
        UpdateCatchImage();
    }

    public void UpdateCatchImage()
    {
        if (State.Elementals.GetElementalEntry(elementalId).isCaught)
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
}