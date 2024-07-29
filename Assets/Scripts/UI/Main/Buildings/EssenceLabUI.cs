using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EssenceLabUI : MonoBehaviour
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
        bool didLevelUp = EssenceLab.LevelUp();
        if (didLevelUp)
        {
            SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);
        }
    }

    public void UpdateUI()
    {
        int EssenceLabLevel = MapManager.Instance.currentMapProgression.essenceLabLevel;

        levelText.text = $"{EssenceLabLevel}/{EssenceLab.currentMapEssenceLabSpecs.MaxLevel}";
        _levelUpImage?.SetActive(!EssenceLab.IsMaxLevel());
        costText.text = EssenceLab.IsMaxLevel() ? "Max" : $"{TextUtil.NumberFormatter(EssenceLab.GetLevelUpCost())}";
        levelUpButton.GetComponent<Button>().interactable = !EssenceLab.IsMaxLevel() && Player.Instance.Resources.Gold >= EssenceLab.GetLevelUpCost();
    }

    private void ScheduleUpdate()
    {
        Invoke("UpdateUI", 0.1f);
    }
}