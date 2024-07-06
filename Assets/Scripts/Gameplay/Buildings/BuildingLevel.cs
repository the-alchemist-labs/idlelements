public class BuildingSpecs
{
    public int BaseBonus { get; }
    public int BaseCost { get;}
    public int MaxLevel { get; }

    public BuildingSpecs(int baseCost, int baseBonus, int maxLevel)
    {   
        BaseBonus = baseBonus;
        MaxLevel = maxLevel;
        BaseCost = baseCost;
    }
}
