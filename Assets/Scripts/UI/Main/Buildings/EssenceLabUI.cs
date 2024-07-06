using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EssenceLabUI : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text costText;
    public GameObject levelUpButton;

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