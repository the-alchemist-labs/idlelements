using System;
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