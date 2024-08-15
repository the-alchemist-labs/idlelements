using System;
using System.Linq;

public class DailyEvents
{
    public void Subscribe()
    {
        // GameEvents.OnGoldUpdated += () => UpdateDailyProgress(DailyObjective.SpendGold, -1);
        GameEvents.OnEssenceUpdated += () => UpdateDailyProgress(DailyObjective.SpendEssence, -1);
    }
    
    private void UpdateDailyProgress(DailyObjective objective, int updateValue)
    {
        
        DailyId dailyId = DailiesManager.Instance.GetDailyByObjective(objective);
        DailiesManager.Instance.UpdateDailyProgress(dailyId, updateValue);
    }
}