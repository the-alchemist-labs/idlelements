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
        DateTime lastTimestamp = savedData.lastTimestamp != null ? DateTime.Parse(savedData.lastTimestamp) : DateTime.Now;
        TimeSpan idleTime = DateTime.Now - lastTimestamp;

        resources.gold += (int)Math.Round(idleTime.TotalSeconds);
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
            resources.gold++;
            print(resources.gold);
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
}
