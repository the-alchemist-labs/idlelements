using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleGains : MonoBehaviour
{
    public HorizontalLayoutGroup goldContainer;
    public TMP_Text goldText;
    public TMP_Text goldPartyBonusText;

    public HorizontalLayoutGroup essenceContainer;
    public TMP_Text essenceText;
    public TMP_Text essencePartyBonusText;

    void Start()
    {
        GameEvents.OnIdleGainsChanged += UpdateIdleGains;
        GameEvents.OnPartyUpdated += UpdateIdleGains;

        UpdateIdleGains();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateIdleGains;
        GameEvents.OnPartyUpdated -= UpdateIdleGains;
    }

    public void UpdateIdleGains()
    {
        int goldGains = GoldMine.GetTotalGoldFromAllMaps() / GoldMine.incomeLoopSeconds;
        float goldPartyBonus = State.party.GetPartyBonusMultipier(BonusResource.Gold);

        goldText.text = $"{TextUtil.NumberFormatter(goldGains)}/s";
        goldPartyBonusText.text = goldPartyBonus != 0 ? $" (+{TextUtil.NumberFormatter((int)(goldGains * goldPartyBonus))})" : "";
        LayoutRebuilder.ForceRebuildLayoutImmediate(goldContainer.GetComponent<RectTransform>());

        int essenceGains = EssenceLab.GetTotalEssenceFromAllMaps() / EssenceLab.incomeLoopSeconds;
        float essencePartyBonus = State.party.GetPartyBonusMultipier(BonusResource.Essence);

        essenceText.text = $"{TextUtil.NumberFormatter(essenceGains)}/s";
        essencePartyBonusText.text = essencePartyBonus != 0 ? $" (+{TextUtil.NumberFormatter((int)(essenceGains * essencePartyBonus))})" : "";
        LayoutRebuilder.ForceRebuildLayoutImmediate(essenceContainer.GetComponent<RectTransform>());

    }
}