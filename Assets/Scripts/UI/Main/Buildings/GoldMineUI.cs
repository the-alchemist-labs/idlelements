using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoldMineUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
            "Gold Mine",
             GoldMine.IsMaxLevel() ? "Next level: 0" : $"Next level: + {GoldMine.GetLevelUpGains()}",
            $"Total gains: {TextUtil.NumberFormatter(GoldMine.GetGoldGain())}",
            $"Collect time: {GoldMine.incomeLoopSeconds} sec",
            "Sprites/Currencies/gold"
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }

    public void LevelUp()
    {
        bool didLevelUp = GoldMine.LevelUp();
        if (didLevelUp)
        {
            levelUpButton.GetComponent<AudioSource>().Play();
        }
    }

    public void UpdateUI()
    {
        int goldMineLevel = State.Maps.currentMapProgression.goldMineLevel;

        levelText.text = $"{goldMineLevel}/{GoldMine.currentGoldMineSpecs.MaxLevel}";
        costText.text = GoldMine.IsMaxLevel() ? "Max" : $"{TextUtil.NumberFormatter(GoldMine.GetLevelUpCost())}";
        levelUpImage?.SetActive(!GoldMine.IsMaxLevel());
        levelUpButton.GetComponent<Button>().interactable = !GoldMine.IsMaxLevel() && State.gold >= GoldMine.GetLevelUpCost();
    }

    private void ScheduleUpdate()
    {
        Invoke("UpdateUI", 0.1f);
    }
}