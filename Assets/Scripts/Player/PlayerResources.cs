public class PlayerResources
{
    public int Essence { get; private set; }
    public int Gold { get; private set; }
    public int Orbs { get; private set; }

    public PlayerResources(int Essence = 20000, int Gold = 20000, int Orbs = 0)
    {
        this.Essence = Essence;
        this.Gold = Gold;
        this.Orbs = Orbs;
    }

    public void UpdateEssence(int amount)
    {
        Essence = (Essence + amount >= 0) ? Essence + amount : 0;
        GameEvents.EssenceUpdated();
    }

    public void UpdateGold(int amount)
    {
        Gold = (Gold + amount >= 0) ? Gold + amount : 0;
        GameEvents.GoldUpdated();
        if (amount < 0) DailyEvents.GoldSpent(amount);
    }

    public void UpdateOrbs(int amount)
    {
        Orbs = (Orbs + amount >= 0) ? Orbs + amount : 0;
    }
}