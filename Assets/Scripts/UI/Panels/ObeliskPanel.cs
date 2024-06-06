using TMPro;
using UnityEngine;

public class ObeliskPanel : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text buffText;
    public TMP_Text levelUpBuff;
    public TMP_Text levelUpCost;
    public GameObject levelUpButton;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateData;
        UpdateData();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateData;
    }

    public void Upgrade()
    {
        Obelisk.LevelUp();
        UpdateData();
    }

    private void UpdateData()
    {
        levelText.text = $"Level: {State.Maps.GetCurrentMapProgresion()?.obeliskLevel}";
        buffText.text = $"+{Obelisk.GetTotalBuff()}% encounter speed";
 
        if (Obelisk.IsMaxLevel())
        {
            levelUpButton.SetActive(false);
        }
        else
        {
            levelUpButton.SetActive(true);
            levelUpBuff.text = $"+{Obelisk.GetNextLevelBuff()}%";
            levelUpCost.text = $"{TextUtil.NumberFormatter(Obelisk.GetLevelUpCost())}";
        }

    }
}
