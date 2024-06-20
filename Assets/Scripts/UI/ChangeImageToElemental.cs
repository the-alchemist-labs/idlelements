using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementaAfkRewardsPrefub : MonoBehaviour
{
    public Image elementalImage;
    public TMP_Text tokensText;

    public void UpdatePrefub(ElementalId id, int amount = 0)
    {   
        Sprite newSprite = Resources.Load<Sprite>($"Sprites/Elementals/{id}");
        elementalImage.sprite = newSprite;
        tokensText.text = (amount != 0) ? $"X{amount}" : "";
    }
}
