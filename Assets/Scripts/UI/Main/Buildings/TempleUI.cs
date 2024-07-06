using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TempleUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Text levelText;
    public TMP_Text costText;
    public GameObject levelUpButton;
    public GameObject infoPanel;

    private GameObject levelUpImage;

    void Start()
    {
        levelUpImage = levelUpButton.transform.GetChild(0).GetComponentInChildren<Image>()?.gameObject;

        GameEvents.OnMapDataChanged += ScheduleUpdate;
        GameEvents.OnGoldUpdated += ScheduleUpdate;

        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= ScheduleUpdate;
        GameEvents.OnGoldUpdated -= ScheduleUpdate;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        infoPanel.GetComponent<BuildingPanel>()?.UpdateUI(
            "Temple",
             Temple.IsMaxLevel() ? "Next level: 0" : $"Next level:  + {Temple.GetLevelUpSpeedBuff()}%",
            $"Total buff: + {TextUtil.NumberFormatter(Temple.GetCurrentMapSpeedGain())}%",
            $"Catch time: {Temple.GetEncounterSpeed()} sec",
            "Sprites/Currencies/time"
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }

    public void LevelUp()
    {
        bool didLevelUp = Temple.LevelUp();
        if (didLevelUp)
        {
            levelUpButton.GetComponent<AudioSource>().Play();
        }
    }

    public void UpdateUI()
    {
        int templeLevel = State.Maps.currentMapProgression.templeLevel;

        levelText.text = $"{templeLevel}/{Temple.currentTempleSpecs.MaxLevel}";
        costText.text = Temple.IsMaxLevel() ? "Max" : $"{TextUtil.NumberFormatter(Temple.GetLevelUpCost())}";
        levelUpImage?.SetActive(!Temple.IsMaxLevel());
        levelUpButton.GetComponent<Button>().interactable = !Temple.IsMaxLevel() && State.gold >= Temple.GetLevelUpCost();
    }

    private void ScheduleUpdate()
    {
        Invoke("UpdateUI", 0.1f);
    }
}