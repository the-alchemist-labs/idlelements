public static class StatsUtil
{
    public static int GetHpStat(int level, int baseStat)
    {
        return (2 * baseStat * level) / 100 + level + 10;
    }
    
    public static int GetStat(int level, int baseStat)
    {
        return (2 * baseStat + 10) / 100 + level + 5;
    }

    public static int CalculateDamage<T, U>(Skill skill, ElementalInstance<T> attacker, ElementalInstance<U> defender)
        where T : IElemental
        where U : IElemental
    {
        return (2 * attacker.Level / 5 + 2) * skill.ImpactValue * (attacker.Attack / defender.Defense) / 50 + 2;
    }
}