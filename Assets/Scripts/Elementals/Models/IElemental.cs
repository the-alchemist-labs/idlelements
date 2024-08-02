using System;
using System.Collections.Generic;

public interface IElemental
{
    public ElementalStats Stats { get; }
    public List<SkillByLevel> Skills { get; }
    public Rewards Rewards { get; }
}

[Serializable]
public class SkillByLevel
{
    public SkillId SkillId;
    public int Level;
}

[Serializable]
public class BallReward
{
    public BallId BallId;
    public int Amount;
    public float Chance;
}

[Serializable]
public class Rewards
{
    public int Gold;
    public int Essence;
    public int Orbs;
    public int Exp;
    public int Token;
    public List<BallReward> Balls;
}
