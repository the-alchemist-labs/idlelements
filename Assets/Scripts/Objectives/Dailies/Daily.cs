using System;
using UnityEngine;

public enum DailyId
{
    First,
    Second,
    Third,
    Forth,
    Fifth,
    Sixth,
    Seventh
}

public enum DailyObjective
{
    EncounterElemental,
    CatchElemental,
    ThrowBalls,
    Login,
    SpendGold,
    SpendEssence
}

[CreateAssetMenu(fileName = "New Daily", menuName = "Scriptable Objects/Objectives/Daily")]
public class Daily : ScriptableObject
{
    public DailyId Id;
    public string Description;
    public int RequiredToComplete;
    public DailyObjective Objective;
    public Reward Reward;
}

[Serializable]
public class DailyProgress
{
    public DailyId Id;
    public int Progress;
    public bool IsCompleted;
    public bool WasClaimed;
    public DateTime? ClaimedAt;



    public DailyProgress(
        DailyId id,
        int? progress,
        bool? isCompleted,
        bool? wasClaimed,
        DateTime? claimedAt
    )
    {
        Id = id;
        Progress = progress ?? 0;
        IsCompleted = isCompleted ?? false;
        WasClaimed = wasClaimed ?? false;
        ClaimedAt = claimedAt;
    }
}