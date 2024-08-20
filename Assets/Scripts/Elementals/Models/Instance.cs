using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ElementalInstance <T> where T : IElemental
{
    public T Base { get; private set; }
    public int Level { get; private set; }
    public List<SkillId> EquippedSkills { get; private set; }
    public int RemainingHp { get; private set; }

    public int Hp => StatsUtil.GetHpStat(Base.Stats.Hp, Level);
    public int Attack => StatsUtil.GetStat(Base.Stats.Attack, Level);
    public int Defense => StatsUtil.GetStat(Base.Stats.Defense, Level);
    public int Speed => StatsUtil.GetStat(Base.Stats.Speed, Level);

    public ElementalInstance(T baseElemental, int level)
    {
        Base = baseElemental;
        Level = level;
        EquippedSkills = GetDefaultSkills(level);
        RemainingHp = Hp;
    }

    public float ReceiveDamage(int damage)
    {
        RemainingHp = RemainingHp - damage < 0 ? 0 : RemainingHp - damage;
        return (float)Hp / (float)RemainingHp;
    }
    public void UpdateSkill(int slot, SkillId skillId)
    {
        EquippedSkills[slot] = skillId;
    }

    private List<SkillId> GetDefaultSkills(int level)
    {
        List<SkillId> topSkills = Base.Skills
            .Where(skill => skill.Level >= level)
            .OrderByDescending(skill => skill.Level)
            .Take(3)
            .Select(skill => skill.SkillId)
            .ToList();
        return topSkills
            .Concat(Enumerable.Repeat<SkillId>(SkillId.None, 3 - topSkills.Count))
            .ToList();
    }
}
