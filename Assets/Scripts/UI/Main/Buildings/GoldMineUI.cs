using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldMineUI : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text costText;
    [SerializeField] GameObject levelUpButton;

    private GameObject _levelUpImage;

    void Start()
    {
        _levelUpImage = levelUpButton.transform.GetChild(0).GetComponentInChildren<Image>()?.gameObject;

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
        bool didLevelUp = GoldMine.LevelUp();
        if (didLevelUp)
        {
            SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);
        }
    }

    public void UpdateUI()
    {
        int goldMineLevel = MapManager.Instance.currentMapProgression.goldMineLevel;

        levelText.text = $"{goldMineLevel}/{GoldMine.currentGoldMineSpecs.MaxLevel}";
        costText.text = GoldMine.IsMaxLevel() ? "Max" : $"{TextUtil.NumberFormatter(GoldMine.GetLevelUpCost())}";
        _levelUpImage?.SetActive(!GoldMine.IsMaxLevel());
        levelUpButton.GetComponent<Button>().interactable = !GoldMine.IsMaxLevel() && Player.Instance.Resources.Gold >= GoldMine.GetLevelUpCost();
    }

    private void ScheduleUpdate()
    {
        Invoke("UpdateUI", 0.1f);
    }
}