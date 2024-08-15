using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DailiesManager : MonoBehaviour
{
    private const string SCRIPTABLE_OBJECT_DAILIES_PATH = "ScriptableObjects/Objectives/Dailies";

    public static DailiesManager Instance;

    private List<DailyProgress> _dailyProgresses;
    private List<Daily> _dailyTasks;
    private DailyEvents _dailyEvents;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            GameEvents.OnDailyUpdated += SaveProgress;
            
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnDailyUpdated -= SaveProgress;
        SaveProgress();
    }

    public List<DailyProgress> GetDailies()
    {
        DailyProgress dailyProgress = _dailyProgresses.First();
        if (IsNewDay(dailyProgress.ClaimedAt))
        {
            ResetDailyProgress();
        }
        
        return SortDailies();
    }

    public Daily GetDaily(DailyId id)
    {
        return _dailyTasks.Single(d => d.Id == id);
    }

    public DailyId GetDailyByObjective(DailyObjective objective)
    {
        return _dailyTasks.Single(d => d.Objective == objective).Id;
    }

    public void UpdateDailyProgress(DailyId dailyId, int progressAmount)
    {
        Daily daily = GetDaily(dailyId);
        DailyProgress dailyProgress =GetDailyProgress(dailyId);
        dailyProgress.Progress += progressAmount;

        dailyProgress.IsCompleted = dailyProgress.Progress >= daily.RequiredToComplete;

        if (dailyProgress.IsCompleted)
        {
            dailyProgress.ClaimedAt = DateTime.Now;
        }

        GameEvents.DailyUpdated();
    }

    public void DailyClaimed(DailyId id)
    {
        DailyProgress dailyProgress = GetDailyProgress(id);
        dailyProgress.WasClaimed = true;
        dailyProgress.ClaimedAt = DateTime.Now;
    }
    
    private void Initialize()
    {
        _dailyTasks = Resources.LoadAll<Daily>(SCRIPTABLE_OBJECT_DAILIES_PATH).ToList();
        List<DailyProgress> progresses = DataService.Instance.LoadData<List<DailyProgress>>(FileName.DailiesProgress, true);
        
        _dailyProgresses = InitDailiesProgress(_dailyTasks, progresses);
        _dailyEvents = new DailyEvents();
        _dailyEvents.Subscribe();
    }

    private List<DailyProgress> InitDailiesProgress(List<Daily> dailyTasks, List<DailyProgress> progresses)
    {
        List<DailyProgress> dailyProgresses = new List<DailyProgress>();
        foreach (Daily daily in dailyTasks)
        {
            DailyProgress progress = progresses.SingleOrDefault(p => p.Id == daily.Id);
            dailyProgresses.Add(new DailyProgress(
                daily.Id,
                progress?.Progress,
                progress?.IsCompleted,
                progress?.WasClaimed,
                progress?.ClaimedAt
            ));
        }

        return dailyProgresses;
    }

    private List<DailyProgress> SortDailies()
    {
        _dailyProgresses.Sort((x, y) =>
        {
            if (x.IsCompleted && !y.IsCompleted || !x.WasClaimed && y.WasClaimed) return -1;
            if (!x.IsCompleted && y.IsCompleted || x.WasClaimed && !y.WasClaimed) return 1;
            return 0;
        });

        return _dailyProgresses;
    }

    private DailyProgress GetDailyProgress(DailyId id)
    {
        return  _dailyProgresses.Single(d => d.Id == id);
    }
    
    private static bool IsNewDay(DateTime? completedDate)
    {
        return (DateTime.Today - completedDate?.Date)?.Days == 1;
    }

    private void ResetDailyProgress()
    {
        foreach (DailyProgress daily in _dailyProgresses)
        {
            daily.Progress = 0;
            daily.IsCompleted = false;
            daily.WasClaimed = false;
        }
        GameEvents.DailyUpdated();
    }

    private void SaveProgress()
    {
        DataService.Instance.SaveData(FileName.DailiesProgress, true, _dailyProgresses);
    }
}