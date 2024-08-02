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