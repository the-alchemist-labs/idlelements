using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class IdleRewards
{
    public int totalCatches = 0;
    public int orbs = 0;
    public int gold = 0;
    public int essence = 0;
    public int experience = 0;
    public List<ElementalId> newCatches = new List<ElementalId>();
    public Dictionary<ElementalId, int> elementalTokens = new Dictionary<ElementalId, int>();
    public ElementalId lastCaught;
    public string IdleTime;
}

public class AfkGainsPanel : MonoBehaviour
{
    public GameObject afkGainsPanel;
    public GameObject newCatchesContainer;
    public GameObject tokensContainer;
    public GameObject elementalPrefab;

    public TMP_Text idleTimeText;
    public TMP_Text essenceText;
    public TMP_Text goldText;
    public TMP_Text expText;

    private IdleRewards rewards;


    void OnDestroy()
    {
    }

    public void DisplayAfkGains()
    {
    }

    public void UpdatePanel()
    {
        afkGainsPanel.SetActive(true);

        idleTimeText.text = $"You idle rewards for {rewards.IdleTime}";
        goldText.text = TextUtil.NumberFormatter(rewards.gold);
        essenceText.text = TextUtil.NumberFormatter(rewards.essence);
        expText.text = TextUtil.NumberFormatter(rewards.experience);
    }

}