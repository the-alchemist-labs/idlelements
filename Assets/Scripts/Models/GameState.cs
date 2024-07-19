using System;
using System.Collections.Generic;

[Serializable]
public struct GameState
{
    public DateTime lastEncounterDate;
    public int level;
    public int experience;
    public int essence;
    public int gold;
    public int orbs;
    public Party party;
}
