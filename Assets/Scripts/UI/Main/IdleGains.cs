using TMPro;
using UnityEngine;

public class IdleGains : MonoBehaviour
{
    public TMP_Text goldText;
    public TMP_Text essenceText;

    void Start()
    {
        GameEvents.OnIdleGainsChanged += UpdateIdleGains;

        UpdateIdleGains();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateIdleGains;
    }

    public void UpdateIdleGains()
    {
        goldText.text = $"{GoldMine.GetTotalGoldFromAllMaps()}/ {GoldMine.incomeLoopSeconds}s";
        essenceText.text = $"{EssenceLab.GetTotalEssenceFromAllMaps()}/ { EssenceLab.incomeLoopSeconds}s";
    }
}