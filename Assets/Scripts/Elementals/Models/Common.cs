using System.Collections.Generic;

public interface IElemental
{
    public ElementalStats Stats { get; }
    public List<SkillByLevel> Skills { get; }
    public Rewards Rewards { get; }
}