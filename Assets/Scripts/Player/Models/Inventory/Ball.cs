using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BallId
{
    None,
    Normal,
    Great,
    Master
}

[Serializable]
public struct CatchByTier
{
    public Tier Tier;
    public float CatchRate;
}

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory/Balls")]
public class Ball : ScriptableObject
{
    public BallId Id;
    public string Name;
    public string Description;
    public List<CatchByTier> CatchByTier;

    public float GetCatchRate(Tier tier)
    {
        return CatchByTier.Find(c => c.Tier == tier).CatchRate;
    }
}
