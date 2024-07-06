using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EssenceLabUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
            "Essence Lab",
            EssenceLab.IsMaxLevel() ? "Next level: 0" : $"Next level: + {EssenceLab.GetLevelUpGains()}",
            $"Total gains: {TextUtil.NumberFormatter(EssenceLab.GetEssenceGain())}",
            $"Collect time: {EssenceLab.incomeLoopSeconds} sec",
            "Sprites/Currencies/essence"
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        infoPanel.SetActive(false);
    }

    public void LevelUp()
    {
        bool didLevelUp = EssenceLab.LevelUp();
        if (didLevelUp)
        {
            levelUpButton.GetComponent<AudioSource>().Play();
        }
    }

    public void UpdateUI()
    {
        int EssenceLabLevel = State.Maps.currentMapProgression.essenceLabLevel;

        levelText.text = $"{EssenceLabLevel}/{EssenceLab.currentMapEssenceLabSpecs.MaxLevel}";
        levelUpImage?.SetActive(!EssenceLab.IsMaxLevel());
        costText.text = EssenceLab.IsMaxLevel() ? "Max" : $"{TextUtil.NumberFormatter(EssenceLab.GetLevelUpCost())}";
        levelUpButton.GetComponent<Button>().interactable = !EssenceLab.IsMaxLevel() && State.gold >= EssenceLab.GetLevelUpCost();
    }

    private void ScheduleUpdate()
    {
        Invoke("UpdateUI", 0.1f);
    }
}