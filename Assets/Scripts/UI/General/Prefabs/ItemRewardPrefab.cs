using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRewardPrefab : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text amountText;

    public void SetItemPrefab(RewardId id, int amount = 0)
    {
        itemImage.sprite = Resources.Load<Sprite>($"Sprites/Inventory/{RewardService.GetItemType(id)}s/{id}");
        amountText.text = (amount != 0) ? $"X{TextUtil.NumberFormatter(amount)}" : "";
    }
}
