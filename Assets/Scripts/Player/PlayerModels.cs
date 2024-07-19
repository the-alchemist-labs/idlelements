using System;
using Newtonsoft.Json;

[Serializable]
public class PlayerInfo
{
    public string id;
    public string friendCode;
    public string name;
    public int level;
    public int elementalsCaught;
    public Party party;
    public bool isOnline;


    public PlayerInfo(string id, string friendCode, string name, int level, int elementalsCaught, Party party, bool isOnline)
    {
        this.id = id;
        this.friendCode = friendCode;
        this.name = name;
        this.level = level;
        this.elementalsCaught = elementalsCaught;
        this.party = party;
        this.isOnline = isOnline;
    }
}

[Serializable]
public class PlayerState
{
    public int Level;
    public int Experience;
    public Party Party;
    public PlayerResources Resources;


    public PlayerState()
    {
        Level = 1;
        Experience = 0;
        Party = new Party();
        Resources = new PlayerResources();
    }
    
    [JsonConstructor]
    public PlayerState(int Level, int Experience, Party Party, PlayerResources Resources)
    {
        this.Level = Level;
        this.Experience = Experience;
        this.Party = Party;
        this.Resources = Resources;
    }
}