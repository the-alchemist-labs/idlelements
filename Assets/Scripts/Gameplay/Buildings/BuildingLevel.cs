public class BuildingLevel
{
    public int BuffBonus { get; }
    public int CostModifier { get;}
    public int MaxLevel { get; }

    public BuildingLevel(int costModifier, int buffBonus, int maxLevel)
    {   
        BuffBonus = buffBonus;
        CostModifier = costModifier;
        MaxLevel = maxLevel;
    }
}
