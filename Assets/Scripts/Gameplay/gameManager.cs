using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private DataService DataService = new DataService();
    private bool encrypted = false; // change to true before release

    private PlayerInfo playerInfo;
    private Resources resources;
    private DateTime lastTimestamp = DateTime.Now;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadData();
        StartCoroutine(ProgressRoutine());
        StartCoroutine(BackupData());
    }

    void Update()
    {
        HeaderBannerManager.Instance.UpdateGoldText(resources.gold.ToString());
    }

    void OnDestroy()
    {
        SaveGameData();
    }

    void LoadData()
    {
        GameData savedData = DataService.LoadData<GameData>("data", encrypted);
        resources = savedData.resources;
        playerInfo = savedData.playerInfo;
        lastTimestamp = savedData.lastTimestamp == null ? lastTimestamp : DateTime.Parse(savedData.lastTimestamp);
        int secondsDiff = GetSecondsDiff(lastTimestamp);
        resources.gold += secondsDiff;
        print("total.gold " + resources.gold);
    }

    void SaveGameData()
    {
        GameData data = new GameData()
        {
            lastTimestamp = DateTime.Now.ToString(),
            resources = resources,
            playerInfo = playerInfo
        };

        DataService.SaveData("data", data, encrypted);
    }

    IEnumerator ProgressRoutine()
    {
        while (true)
        {
            resources.gold+= GetSecondsDiff(lastTimestamp);
            lastTimestamp = DateTime.Now;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator BackupData()
    {
        while (true)
        {
            SaveGameData();
            yield return new WaitForSeconds(5);
        }
    }

    int GetSecondsDiff(DateTime date)
    {
        TimeSpan diff = DateTime.Now - date;
        return (int)diff.TotalSeconds;
    }
}
