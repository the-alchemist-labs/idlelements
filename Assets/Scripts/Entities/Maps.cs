using System.Diagnostics;

public static class Maps {
    public static Map mapA = new Map("map a", 1, new ElementalEncounter[] {
        new ElementalEncounter(Elementals.elementalA, 0.3f),
        new ElementalEncounter(Elementals.elementalB, 0.7f),
    });

    public static Map[] versionAMaps = { mapA };
}