using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private DataService DataService = new DataService();
    private bool encrypted = false; // change to true before release

    private PlayerInfo playerInfo;
    private Resources resources;
    private DateTime lastEncounter = DateTime.Now;
    private Map currentMap = Maps.mapA;

    private int encounterRate = 10;

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
        lastEncounter = savedData.lastEncounter == null ? lastEncounter : DateTime.Parse(savedData.lastEncounter);
        int secondsDiff = GetSecondsDiff(lastEncounter);
        resources.gold += secondsDiff;
    }

    void SaveGameData()
    {
        GameData data = new GameData()
        {
            lastEncounter = DateTime.Now.ToString(),
            resources = resources,
            playerInfo = playerInfo
        };

        DataService.SaveData("data", data, encrypted);
    }

    IEnumerator ProgressRoutine()
    {
        while (true)
        {
            int elapedsSecconds = GetSecondsDiff(lastEncounter);
            int encounters = elapedsSecconds / encounterRate;
            int remainder = elapedsSecconds % encounterRate;

            if (encounters > 0)
            {
                Enumerable.Range(0, encounters).ToList().ForEach(_ => TriggerEncounter());
                lastEncounter = DateTime.Now.AddSeconds(remainder);
            }

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

    void TriggerEncounter()
    {
        Elemental elemental = currentMap.GetEncounter();
        bool isCaught = elemental.IsCaught(); // add modifiers
        if (isCaught)
        {
            resources.gold += elemental.goldGain;
            elemental.tokens += elemental.isRegistered ? 1 : 0;
            elemental.isRegistered = true;
        }
    }
}
