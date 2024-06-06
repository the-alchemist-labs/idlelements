using System.Collections.Generic;

public class ObeliskLevel
{
    public int CostModifier { get; set; }
    public int BuffBonus { get; set; }

    public ObeliskLevel(int costModifier, int buffBonus)
    {
        CostModifier = costModifier;
        BuffBonus = buffBonus;
    }
}

public static class Obelisk
{
    private static int MAX_LEVEL = 10;

    public readonly static Dictionary<MapId, ObeliskLevel> baseModifiers = new Dictionary<MapId, ObeliskLevel>()
    {
        { MapId.MapA, new ObeliskLevel(100, 5) },
        { MapId.MapB, new ObeliskLevel(500, 10) },
        { MapId.MapC, new ObeliskLevel(1000, 10) },
    };

    public static bool IsMaxLevel()
    {
        return State.Maps.GetCurrentMapProgresion().obeliskLevel >= MAX_LEVEL;
    }

    public static void LevelUp()
    {
        if (IsMaxLevel())
        {
            return;
        }

        int levelUpCost = GetLevelUpCost();
        State.UpdateEssence(-levelUpCost);
        State.Maps.GetCurrentMapProgresion().obeliskLevel++;
    }

    public static int GetTotalBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        if (map.obeliskLevel == 0) return 0;
        return 100 + map.obeliskLevel * baseModifiers[State.Maps.currentMapId].BuffBonus;
    }

    public static int GetNextLevelBuff()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        ObeliskLevel obeliskLevel = baseModifiers[State.Maps.currentMapId];
        return (map.obeliskLevel + 1) * obeliskLevel.BuffBonus;
    }

    public static int GetLevelUpCost()
    {
        MapProgression map = State.Maps.GetCurrentMapProgresion();
        ObeliskLevel obeliskLevel = baseModifiers[State.Maps.currentMapId];
        return (map.obeliskLevel + 1) * obeliskLevel.CostModifier;
    }
}