using System;
using Newtonsoft.Json;

[Serializable]
public class PlayerState
{
    public int Level;
    public int Experience;
    public Party Party;
    public PlayerResources Resources;
    public Inventory Inventory;

    public PlayerState()
    {
        Level = 1;
        Experience = 0;
        Party = new Party();
        Resources = new PlayerResources();
        Inventory = new Inventory();
    }
    
    [JsonConstructor]
    public PlayerState(int Level, int Experience, Party Party, PlayerResources Resources, Inventory Inventory)
    {
        this.Level = Level;
        this.Experience = Experience;
        this.Party = Party;
        this.Resources = Resources;
        this.Inventory = Inventory;
    }
}