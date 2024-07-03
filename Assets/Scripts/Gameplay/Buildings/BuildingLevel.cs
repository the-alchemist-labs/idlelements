public class BuildingSpecs
{

    public int BuffBonus { get; }
    public int CostModifier { get;}
    public int MaxLevel { get; }

    public BuildingSpecs(int costModifier, int buffBonus, int maxLevel)
    {   
        BuffBonus = buffBonus;
        CostModifier = costModifier;
        MaxLevel = maxLevel;
    }
}
