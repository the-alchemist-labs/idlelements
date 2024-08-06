using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRewardPrefab : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text amountText;

    public void SetItemPrefab(BallId id, int amount = 0)
    {
        itemImage.sprite = Resources.Load<Sprite>($"Sprites/Inventory/Balls/{id}");
        amountText.text = (amount != 0) ? $"X{amount}" : "";
    }

    public void SetItemPrefab(ElementType type, int amount = 0)
    {
        itemImage.sprite = Resources.Load<Sprite>($"Sprites/Inventory/Elementokens/{type}");
        amountText.text = (amount != 0) ? $"X{amount}" : "";
    }
}
