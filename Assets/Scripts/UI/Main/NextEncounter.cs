using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextEncounter : MonoBehaviour
{
    public Slider encounterSlider;
    public TMP_Text secondsToEncounterText;
    public TMP_Text boostButtonText;
    public Button boostButton;
    public AudioSource boostSound;

    private Coroutine sliderCoroutine;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateBoostText;
        GameEvents.OnIdleGainsChanged += UpdateBoostText;
        GameEvents.OnEssenceUpdated += UpdateBoostButton;
        GameEvents.OnPlayerInitialized += StartSliderUpdate;
        UpdateBoostText();
    }

    void OnEnable()
    {
        GameManager gameManger = GameObject.FindGameObjectWithTag(Tags.GameManager).GetComponent<GameManager>();
        if (gameManger.IsReady())
        {
            StartSliderUpdate();
        }
    }

    void OnDisable()
    {
        if (sliderCoroutine != null)
        {
            StopCoroutine(sliderCoroutine);
        }
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateBoostText;
        GameEvents.OnIdleGainsChanged -= UpdateBoostText;
        GameEvents.OnEssenceUpdated -= UpdateBoostButton;
        GameEvents.OnPlayerInitialized -= StartSliderUpdate;
    }

    void StartSliderUpdate()
    {
        sliderCoroutine = StartCoroutine(UpdateReactiveData());

    }

    IEnumerator UpdateReactiveData()
    {
        while (true)
        {
            Invoke("UpdateEncounterSliderData", 0.1f);
            yield return new WaitForSeconds(1f);
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

    public void Boost()
    {
        if (Player.Instance.Resources.Essence >= Temple.GetBoostCost())
        {
            Temple.Boost();
            UpdateEncounterSliderData();
            boostSound.Play();
        }
    }

    void UpdateBoostButton()
    {
        boostButton.interactable = Player.Instance.Resources.Essence >= Temple.GetBoostCost();
    }

    void UpdateBoostText()
    {
        boostButtonText.text = $"Boost {Temple.currentTempleSpecs.BoostEffect}S / {TextUtil.NumberFormatter(Temple.GetBoostCost())}";
    }
}
