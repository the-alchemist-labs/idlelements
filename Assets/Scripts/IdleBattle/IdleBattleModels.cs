using System;
using Newtonsoft.Json;

[Serializable]
public class IdleBattleManagerState
{
    public int CurrentStage { get; }

    public IdleBattleManagerState()
    {
        CurrentStage = 1;
    }

    [JsonConstructor]
    public IdleBattleManagerState(int CurrentStage)
    {
        this.CurrentStage = CurrentStage;
    }
}