using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldMineUI : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text costText;
    public GameObject levelUpButton;
    public AudioSource levelUpSound;

    private GameObject levelUpImage;

    void Start()
    {
        levelUpImage = levelUpButton.transform.GetChild(0).GetComponentInChildren<Image>()?.gameObject;

        GameEvents.OnMapDataChanged += ScheduleUpdate;
        GameEvents.OnGoldUpdated += ScheduleUpdate;
        GameEvents.OnPlayerInitialized += UpdateUI;
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= ScheduleUpdate;
        GameEvents.OnGoldUpdated -= ScheduleUpdate;
        GameEvents.OnPlayerInitialized -= UpdateUI;
    }

    public void LevelUp()
    {
        bool didLevelUp = GoldMine.LevelUp();
        if (didLevelUp)
        {
            levelUpSound.Play();
        }
    }

    public void UpdateUI()
    {
        int goldMineLevel = MapManager.Instance.currentMapProgression.goldMineLevel;

        levelText.text = $"{goldMineLevel}/{GoldMine.currentGoldMineSpecs.MaxLevel}";
        costText.text = GoldMine.IsMaxLevel() ? "Max" : $"{TextUtil.NumberFormatter(GoldMine.GetLevelUpCost())}";
        levelUpImage?.SetActive(!GoldMine.IsMaxLevel());
        levelUpButton.GetComponent<Button>().interactable = !GoldMine.IsMaxLevel() && Player.Instance.Resources.Gold >= GoldMine.GetLevelUpCost();
    }

    private void ScheduleUpdate()
    {
        Invoke("UpdateUI", 0.1f);
    }
}