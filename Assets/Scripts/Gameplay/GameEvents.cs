using System;

public static class GameEvents
{
    public static event Action OnMapDataChanged;

    public static void MapDataChanged()
    {
        OnMapDataChanged?.Invoke();
    }
}