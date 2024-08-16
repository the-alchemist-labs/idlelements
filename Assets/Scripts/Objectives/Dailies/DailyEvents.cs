using System;
using System.Linq;

public static class DailyEvents
{
    private static event Action<int> _onGoldSpent; 
    private static event Action _onNewEncounter; 

    public static void Subscribe()
    {
        _onGoldSpent += (int amount) => UpdateDailyProgress(DailyObjective.SpendEssence, amount);
        _onNewEncounter += () => UpdateDailyProgress(DailyObjective.EncounterElemental, 1);
    }

    public static void GoldSpent(int amount)
    {
        _onGoldSpent?.Invoke(Math.Abs(amount));
    }
    
    public static void NewEncounter()
    {
        _onNewEncounter?.Invoke();
    }
    
    private static void UpdateDailyProgress(DailyObjective objective, int updateValue)
    {
        DailyId dailyId = DailiesManager.Instance.GetDailyByObjective(objective);
        DailiesManager.Instance.UpdateDailyProgress(dailyId, updateValue);
    }
    
}