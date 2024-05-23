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
        print(resources.diamonds);
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
