public class BuildingLevel
{

    public int interval { get; }
    public int BuffBonus { get; }
    public int CostModifier { get;}
    public int MaxLevel { get; }

    public BuildingLevel(int interval, int costModifier, int buffBonus, int maxLevel)
    {   
        this.interval = interval;
        BuffBonus = buffBonus;
        CostModifier = costModifier;
        MaxLevel = maxLevel;
    }
}
