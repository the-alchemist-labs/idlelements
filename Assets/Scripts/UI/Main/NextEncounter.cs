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
        GameEvents.OnIdleGainsChanged += UpdateBoostText;
        GameEvents.OnEssenceUpdated += UpdateBoostButton;
        UpdateBoostText();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateBoostText;
        GameEvents.OnIdleGainsChanged -= UpdateBoostText;
        GameEvents.OnEssenceUpdated -= UpdateBoostButton;
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
            Invoke("UpdateEncounterSliderData", 0.1f);
            yield return new WaitForSeconds(0.1f);
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

    void UpdateBoostButton()
    {
        boostButton.interactable = State.essence >= Temple.GetBoostCost();
    }

    void UpdateBoostText()
    {
        boostButtonText.text = $"Boost {Temple.currentTemple.BoostEffect}s/{Temple.GetBoostCost()}";
    }
}
