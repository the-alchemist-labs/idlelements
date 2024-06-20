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
    }

    void OnEnable()
    {
        StartCoroutine(UpdateReactiveData());
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
            boostButton.interactable = State.essence >= Temple.BOOST_SECONDS;
            UpdateEncounterSliderData();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void UpdateEncounterSliderData()
    {
        int secondsUntilNextEncounter = State.GetSecondsUntilNextEncounter();
        secondsToEncounterText.text = $"{Math.Max(secondsUntilNextEncounter, 0)}/{State.GetEncounterSpeed()}";
        encounterSlider.value = 1 - ((float)secondsUntilNextEncounter / (float)State.GetEncounterSpeed());
    }
}
