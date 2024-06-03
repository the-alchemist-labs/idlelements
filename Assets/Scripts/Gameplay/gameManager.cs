using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject afkGainsPanel;
    private static GameManager instance;
    private int encounterRate = 5;

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
        HandleAfkGainsPanel();
        StartCoroutine(ProgressRoutine());
        StartCoroutine(Backup());
    }

    void OnDestroy()
    {
        State.Save();
    }

    void HandleAfkGainsPanel()
    {
        int elapedsSecconds = GetSecondsDiff(State.lastEncounter);
        int encounters = elapedsSecconds / encounterRate;
        IdleRewards rewards = GameActions.TriggerMultipleEncounters(encounters);
        State.UpdateLastEncounter(DateTime.Now);

        if (afkGainsPanel.TryGetComponent(out AfkGainsPanel panel))
        {
            panel.DisaplyAfkGains(rewards);
        }
    }

    IEnumerator ProgressRoutine()
    {
        while (true)
        {
            int elapedsSecconds = GetSecondsDiff(State.lastEncounter);
            int encounters = elapedsSecconds / encounterRate;

            if (encounters > 0)
            {
                IdleRewards rewards = encounters == 1
                ? GameActions.TriggerEncounter()
                : GameActions.TriggerMultipleEncounters(encounters);
                GameActions.EarnRewardOfEncounters(rewards);
            }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Backup()
    {
        State.Save();
        yield return new WaitForSeconds(1);
    }

    int GetSecondsDiff(DateTime date)
    {
        TimeSpan diff = DateTime.Now - date;
        return Math.Min((int)diff.TotalSeconds, Conts.MaxIdleSecond);
    }
}
