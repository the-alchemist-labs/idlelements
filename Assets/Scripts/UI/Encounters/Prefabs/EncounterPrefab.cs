using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterPrefub : MonoBehaviour
{
    public HorizontalLayoutGroup layoutGroup;
    public Image elementalImage;
    public TMP_Text elementalId;
    public TMP_Text elementalName;
    public Image typeImage;
    public TMP_Text chanceText;
    public TMP_Text tokensText;

    public void UpdateIU(ElementalId id, ElementType type, string name, float chance, int tokens)
    {
        Color background = new Color(0.5f, 0, 0, 0.5f);
        gameObject.GetComponent<Image>().color = background;
        elementalId.text = $"#{(int)id}";
        elementalName.text = name;
        chanceText.text = $"Chance: {chance * 100}%";
        tokensText.text = $"Tokens: {tokens}";

        Sprite newSprite = Resources.Load<Sprite>($"Sprites/Elementals/{id}");
        elementalImage.sprite = newSprite;

        Sprite newType = Resources.Load<Sprite>($"Sprites/Types/{type}");
        typeImage.sprite = newType;
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
    }

}
