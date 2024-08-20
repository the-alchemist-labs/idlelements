using System;
using System.Collections.Generic;

public interface IElemental
{
    public ElementalStats Stats { get; }
    public List<SkillByLevel> Skills { get; }
    public List<Reward> Rewards { get; }
}

[Serializable]
public class SkillByLevel
{
    public SkillId SkillId;
    public int Level;
}

[Serializable]
public class ElementalStats
{
    public int Hp;
    public int Attack;
    public int Defense;
    public int Speed;
}