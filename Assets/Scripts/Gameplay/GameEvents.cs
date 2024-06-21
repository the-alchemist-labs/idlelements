using System;

public static class GameEvents
{
    public static event Action OnMapDataChanged;
    public static event Action OnIdleGainsChanged;

    public static void MapDataChanged()
    {
        OnMapDataChanged?.Invoke();
    }

    public static void IdleGainsChanged()
    {
        OnIdleGainsChanged?.Invoke();
    }
}