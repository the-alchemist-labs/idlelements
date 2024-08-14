
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
public class Daily: ScriptableObject
{
    public DailyId Id;
    public string Task;
    public int RequiredToComplete;
    public DailyObjective Objective;
    public Reward Reward;
}

[Serializable]
public class DailyProgress
{
    public string Id;
    public DateTime CompletedDate;
    public int Progress;
    public bool WasClaimed;
}