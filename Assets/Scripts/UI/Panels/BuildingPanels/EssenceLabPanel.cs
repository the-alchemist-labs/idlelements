using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EssenceLabPanel : MonoBehaviour
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

    void OnEnable()
    {
        StartCoroutine(UpdateReactiveData());
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateData;
    }

    public void Upgrade()
    {
        EssenceLab.LevelUp();
        UpdateData();
    }

    private void UpdateData()
    {
        levelText.text = $"Level: {State.Maps.GetCurrentMapProgresion()?.essenceLabLevel}";
        if (EssenceLab.IsMaxLevel())
        {
            levelUpButton.SetActive(false);
        }
        else
        {
            levelUpButton.SetActive(true);
            levelUpCost.text = $"{TextUtil.NumberFormatter(EssenceLab.GetLevelUpCost())}";
        }

    }

    IEnumerator UpdateReactiveData()
    {
        while (true)
        {
            levelUpButton.GetComponent<Button>().interactable = State.essence >= EssenceLab.GetLevelUpCost();
            UpdateIncomeSliderData();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void UpdateIncomeSliderData()
    {
        if (State.Maps.currentMapProgression.essenceLabLevel == 0)
        {
            secondsToCollectText.text = "0";
            collectSlider.value = 1;
            return;
        }

        secondsToCollectText.text = $"{EssenceLab.GetTotalBuff()}";
        collectSlider.value = ((float)(EssenceLab.GetSecondsSinceLastCollect()) / (float)EssenceLab.incomeLoopSeconds);
    }

}
