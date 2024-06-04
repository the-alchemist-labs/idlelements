using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scrollViewDeckEntry : MonoBehaviour
{
    public Image elementalImage;
    public TMP_Text elementalId;
    public TMP_Text elementalName;
    public Image caughtIndicator;
    public TMP_Text tokensText;

    public void UpdateEntry(Elemental elemental)
    {
        ElementalEntry entry = State.Elementals.GetElementalEntry(elemental.id);
        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        elementalId.text = $"#{(int)elemental.id}";
        elementalName.text = elemental.name;
        tokensText.text = $"Tokens: {entry.tokens}";
        
        string caughtIndicatorPath = entry.isCaught ? "Sprites/UI/caught" : "Sprites/UI/not-caught";
        caughtIndicator.sprite = Resources.Load<Sprite>(caughtIndicatorPath);
    }
}