public class BuildingLevel
{
    public int UnlockBonus { get; set; }
    public int BuffBonus { get; set; }
    public int CostModifier { get; set; }

    public BuildingLevel(int costModifier, int buffBonus, int unlockBonus = 0)
    {   
        BuffBonus = buffBonus;
        CostModifier = costModifier;
        UnlockBonus = unlockBonus;
    }
}
