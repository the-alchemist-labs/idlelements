using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text nextLevelBonusText;
    public TMP_Text totalGainsText;
    public TMP_Text intervalText;
    public Image totalCurrenyImage;
    public Image bonusCurrenyImage;

    public GameObject darkBgPanel;

    void OnDisable()
    {
        darkBgPanel.SetActive(false);
    }

    public void UpdateUI(string title, string bonusEffectString, string totalBonusString, string intervalString, string imageUrl)
    {
        gameObject.SetActive(true);
        darkBgPanel.SetActive(true);

        titleText.text = title;
        nextLevelBonusText.text = bonusEffectString;
        totalGainsText.text = totalBonusString;
        intervalText.text = intervalString;

        if (imageUrl == null)
        {
            totalCurrenyImage.gameObject.SetActive(false);
            bonusCurrenyImage.gameObject.SetActive(false);
        }
        else
        {
            totalCurrenyImage.sprite = Resources.Load<Sprite>(imageUrl);
            bonusCurrenyImage.sprite = Resources.Load<Sprite>(imageUrl);
        }
    }
}