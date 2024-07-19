using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempleUI : MonoBehaviour
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

        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= ScheduleUpdate;
        GameEvents.OnGoldUpdated -= ScheduleUpdate;
    }

    public void LevelUp()
    {
        bool didLevelUp = Temple.LevelUp();
        if (didLevelUp)
        {
            levelUpSound.Play();
        }
    }

    public void UpdateUI()
    {
        int templeLevel = MapManager.Instance.currentMapProgression.templeLevel;

        levelText.text = $"{templeLevel}/{Temple.currentTempleSpecs.MaxLevel}";
        costText.text = Temple.IsMaxLevel() ? "Max" : $"{TextUtil.NumberFormatter(Temple.GetLevelUpCost())}";
        levelUpImage?.SetActive(!Temple.IsMaxLevel());
        levelUpButton.GetComponent<Button>().interactable = !Temple.IsMaxLevel() && Player.Instance.Resources.Gold >= Temple.GetLevelUpCost();
    }

    private void ScheduleUpdate()
    {
        Invoke("UpdateUI", 0.1f);
    }
}