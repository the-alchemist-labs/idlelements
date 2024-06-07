using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TemplePanel : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text buffText;
    public TMP_Text levelUpBuff;
    public TMP_Text levelUpCost;
    public TMP_Text boostCostText;
    public GameObject levelUpButton;
    public Button boostButton;
    public Slider encounterSlider;
    public TMP_Text secondsToEncounterText;

    private int boostCost = 10;

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
        Temple.LevelUp();
        UpdateData();
    }

    public void Boost()
    {
        Temple.Boost();
        UpdateEncounterSliderData();
    }

    private void UpdateData()
    {
        boostCostText.text = boostCost.ToString();
        levelText.text = $"Level: {State.Maps.GetCurrentMapProgresion()?.templeLevel}";
        buffText.text = $"+{Temple.GetTotalBuff()}% encounter speed";
        if (Temple.IsMaxLevel())
        {
            levelUpButton.SetActive(false);
        }
        else
        {
            levelUpButton.SetActive(true);
            levelUpBuff.text = $"+{Temple.GetNextLevelBuff()}%";
            levelUpCost.text = $"{TextUtil.NumberFormatter(Temple.GetLevelUpCost())}";
        }

    }

    IEnumerator UpdateReactiveData()
    {
        while (true)
        {
            levelUpButton.GetComponent<Button>().interactable = State.gold >= Temple.GetLevelUpCost();
            boostButton.interactable = State.essence >= boostCost;
            UpdateEncounterSliderData();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void UpdateEncounterSliderData()
    {
        int secondsUntilNextEncounter = State.GetSecondsUntilNextEncounter();
        secondsToEncounterText.text = $"{secondsUntilNextEncounter} seconds";
        encounterSlider.value = 1 - ((float)secondsUntilNextEncounter / (float)State.GetEncounterSpeed());
    }
}
