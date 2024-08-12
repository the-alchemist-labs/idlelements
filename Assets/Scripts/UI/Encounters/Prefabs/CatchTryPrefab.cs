using Encounters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatchTryPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text tryText;
    [SerializeField] private Image failedImage;
    [SerializeField] private Image selectedIndicatorImage;

    [SerializeField] private HorizontalLayoutGroup costContainer;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Image costImage;

    public void Init(int tryNum, CatchTryCost cost, bool isSelected, bool isFailed)
    {
        bool hasCurrency = cost.Currency != Resource.None;
        bool isFree = cost.Cost == 0;

        tryText.text = $"{tryNum + 1}";

        failedImage.gameObject.SetActive(isFailed);
        selectedIndicatorImage.gameObject.SetActive(isSelected);
        selectedIndicatorImage.color = EncounterManger.Instance.Encounter.State == EncounterState.Caught
            ? Color.green
            : Color.white;
        costText.text = isFree ? "Free" : $"{TextUtil.NumberFormatter(cost.Cost)}";
        costImage.gameObject.SetActive(hasCurrency);

        if (hasCurrency)
        {
            costImage.sprite = Resources.Load<Sprite>($"Sprites/Currencies/{cost.Currency}");
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(costContainer.GetComponent<RectTransform>());
    }
}