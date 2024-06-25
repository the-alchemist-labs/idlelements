public class BuildingLevel
{

    public int Interval { get; }
    public int BuffBonus { get; }
    public int CostModifier { get;}
    public int MaxLevel { get; }

    public BuildingLevel(int interval, int costModifier, int buffBonus, int maxLevel)
    {   
        Interval = interval;
        BuffBonus = buffBonus;
        CostModifier = costModifier;
        MaxLevel = maxLevel;
    }
}
