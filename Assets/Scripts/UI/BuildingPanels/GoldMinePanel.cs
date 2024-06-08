using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldMinePanel : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text levelUpBuff;
    public TMP_Text levelUpCost;
    public GameObject levelUpButton;
    public Slider collectSlider;
    public TMP_Text secondsToCollectText;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateData;
        StartCoroutine(UpdateReactiveData());
        UpdateData();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateData;
    }

    public void Upgrade()
    {
        GoldMine.LevelUp();
        UpdateData();
    }

    private void UpdateData()
    {
        levelText.text = $"Level: {State.Maps.GetCurrentMapProgresion()?.goldMineLevel}";
        if (GoldMine.IsMaxLevel())
        {
            levelUpButton.SetActive(false);
        }
        else
        {
            levelUpButton.SetActive(true);
            levelUpBuff.text = $"+{GoldMine.GetNextLevelBuff()}";
            levelUpCost.text = $"{TextUtil.NumberFormatter(GoldMine.GetLevelUpCost())}";
        }

    }

    IEnumerator UpdateReactiveData()
    {
        while (true)
        {
            levelUpButton.GetComponent<Button>().interactable = State.gold >= GoldMine.GetLevelUpCost();
            UpdateIncomeSliderData();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void UpdateIncomeSliderData()
    {
        if (State.Maps.currentMapProgression.goldMineLevel == 0)
        {
            secondsToCollectText.text = "0";
            collectSlider.value = 1;
            return;
        }

        secondsToCollectText.text = $"{GoldMine.GetTotalBuff()}";
        collectSlider.value = ((float)(GoldMine.GetSecondsSinceLastCollect()) / (float)GoldMine.incomeLoopSeconds);
    }
}
