using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class IdleBattleManagerState
{
    public int CurrentStage { get; }
    public DateTime LastRewardTimestamp { get; }

    public IdleBattleManagerState()
    {
        CurrentStage = 1;
        LastRewardTimestamp = DateTime.Now;
    }

    [JsonConstructor]
    public IdleBattleManagerState(int CurrentStage, DateTime? LastRewardTimestamp)
    {
        this.CurrentStage = CurrentStage;
        this.LastRewardTimestamp = LastRewardTimestamp ?? DateTime.Now;
    }
}

public class IdleRewards
{
    public int Gold = 0;
    public int Essence = 0;
    public int Experience = 0;
    public Dictionary<BallId, int> Balls = new Dictionary<BallId, int>();
    public Dictionary<ElementType, int> Elementokens = new Dictionary<ElementType, int>();
    public string IdleTime;
}
