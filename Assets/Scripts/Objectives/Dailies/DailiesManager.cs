using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DailiesManager: MonoBehaviour
{
    private const string SCRIPTABLE_OBJECT_DAILIES_PATH = "ScriptableObjects/Objectives/Daily";

    public static DailiesManager Instance;
    
    private List<Daily> _dailyTasks;
    private List<DailyProgress> _progress;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        _dailyTasks = Resources.LoadAll<Daily>(SCRIPTABLE_OBJECT_DAILIES_PATH).ToList();
        _progress = DataService.Instance.LoadData<List<DailyProgress>>(FileName.DailiesProgress, true);
    }
}
