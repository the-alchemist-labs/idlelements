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
        float goldPartyBonusMultiplier = Player.Instance.Party.GetPartyBonusMultipier(BonusResource.Gold);
        int goldGains = GoldMine.GetTotalGoldGains();
        int goldPartyBonus = Mathf.FloorToInt(goldGains * goldPartyBonusMultiplier);

        goldText.text = $"{TextUtil.NumberFormatter(goldGains - goldPartyBonus)}/s";
        goldPartyBonusText.text = goldPartyBonusMultiplier != 0 ? $" (+{TextUtil.NumberFormatter(goldPartyBonus)})" : "";
        LayoutRebuilder.ForceRebuildLayoutImmediate(goldContainer.GetComponent<RectTransform>());

        float essencePartyBonusMultiplier = Player.Instance.Party.GetPartyBonusMultipier(BonusResource.Essence);
        int essenceGains = EssenceLab.GetTotalEssenceFromAllMaps();
        int essencePartyBonus = Mathf.FloorToInt(essenceGains * essencePartyBonusMultiplier);

        essenceText.text = $"{TextUtil.NumberFormatter(essenceGains - essencePartyBonus)}/s";
        essencePartyBonusText.text = essencePartyBonus != 0 ? $" (+{TextUtil.NumberFormatter(essencePartyBonus)})" : "";
        LayoutRebuilder.ForceRebuildLayoutImmediate(essenceContainer.GetComponent<RectTransform>());
    }
}