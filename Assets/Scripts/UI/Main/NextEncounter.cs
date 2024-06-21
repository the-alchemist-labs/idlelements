using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextEncounter : MonoBehaviour
{
    public TMP_Text boostButtonText;
    public Button boostButton;
    public Slider encounterSlider;
    public TMP_Text secondsToEncounterText;

    void Start()
    {
        StartCoroutine(UpdateReactiveData());
        GameEvents.OnMapDataChanged += UpdateBoostText;
        UpdateBoostText();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateBoostText;
    }

    public void Boost()
    {
        Temple.Boost();
        UpdateEncounterSliderData();
    }

    IEnumerator UpdateReactiveData()
    {
        while (true)
        {
            boostButton.interactable = State.essence >= Temple.GetBoostCost();
            UpdateEncounterSliderData();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void UpdateEncounterSliderData()
    {
        int secondsUntilNextEncounter = Temple.GetSecondsUntilNextEncounter() % Temple.GetEncounterSpeed();
        secondsToEncounterText.text = secondsUntilNextEncounter <= 0
        ? "Catch!"
        : $"{Math.Max(secondsUntilNextEncounter, 0)}/{Temple.GetEncounterSpeed()}";
        encounterSlider.value = 1 - ((float)secondsUntilNextEncounter / (float)Temple.GetEncounterSpeed());
    }

    void UpdateBoostText()
    {
        boostButtonText.text = $"Boost {Temple.GetBoostEffect()}s/{Temple.GetBoostCost()}";
    }
}
