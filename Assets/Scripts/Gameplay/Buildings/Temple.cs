using System.Collections.Generic;

public class TempleLevel
{
    public int UnlockBonus { get; set; }
    public int BuffBonus { get; set; }
    public int CostModifier { get; set; }

    public TempleLevel(int unlockBonus, int costModifier, int buffBonus)
    {   
        UnlockBonus = unlockBonus;
        BuffBonus = buffBonus;
        CostModifier = costModifier;
    }
}

public static class Temple
{
    private static int MAX_LEVEL = 10;
    private static int BOOST_SECONDS = 10;

    public readonly static Dictionary<MapId, TempleLevel> baseModifiers = new Dictionary<MapId, TempleLevel>()
    {
        { MapId.MapA, new TempleLevel(100, 100, 10) },
        { MapId.MapB, new TempleLevel(100, 500, 10) },
        { MapId.MapC, new TempleLevel(100, 1000, 10) },
    };

    public static bool IsMaxLevel()
    {
        return State.Maps.GetCurrentMapProgresion().templeLevel >= MAX_LEVEL;
    }

    public static void LevelUp()
    {
        if (IsMaxLevel())
        {
            return;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateGold(-levelUpCost);
        State.Maps.GetCurrentMapProgresion().templeLevel++;
    }

    public static void Boost()
    {
        State.UpdateLastEncounter(State.lastEncounter.AddSeconds(-BOOST_SECONDS));
        State.UpdateEssence(-BOOST_SECONDS);
    }

    public static int GetTotalBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        if (map.templeLevel == 0) return 0;

        return baseModifiers[State.Maps.currentMapId].UnlockBonus 
        + map.templeLevel * baseModifiers[State.Maps.currentMapId].BuffBonus;
    }

    public static int GetNextLevelBuff()
    {  
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        TempleLevel templeLevel = baseModifiers[State.Maps.currentMapId];

        if (map.templeLevel == 0) return templeLevel.UnlockBonus;
        return templeLevel.BuffBonus;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        TempleLevel templeLevel = baseModifiers[State.Maps.currentMapId];
        return (map.templeLevel + 1) * templeLevel.CostModifier;
    }
}