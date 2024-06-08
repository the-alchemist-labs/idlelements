using System.Collections.Generic;

public static class Temple
{
    private static int MAX_LEVEL = 10;
    private static int BOOST_SECONDS = 10;

    public readonly static Dictionary<MapId, BuildingLevel> baseModifiers = new Dictionary<MapId, BuildingLevel>()
    {
        { MapId.MapA, new BuildingLevel(100, 10, 100) },
        { MapId.MapB, new BuildingLevel(500, 10, 100) },
        { MapId.MapC, new BuildingLevel(1000, 10, 100) },
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
        BuildingLevel templeLevel = baseModifiers[State.Maps.currentMapId];

        if (map.templeLevel == 0) return templeLevel.UnlockBonus;
        return templeLevel.BuffBonus;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        BuildingLevel templeLevel = baseModifiers[State.Maps.currentMapId];
        return (map.templeLevel + 1) * templeLevel.CostModifier;
    }
}