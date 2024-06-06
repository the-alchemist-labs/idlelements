using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject afkGainsPanel;
    private static GameManager instance;
    private int encounterSpeed = 10;

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
        int encounters = GetEncounters();
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
            int encounters = GetEncounters();
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

    int GetSecondsSinceLastEncounter(DateTime date)
    {
        TimeSpan diff = DateTime.Now - date;
        return Math.Min((int)diff.TotalSeconds, Conts.MaxIdleSecond);
    }

    int GetEncounters()
    {
        float encounterSpeedModifier = Obelisk.GetTotalBuff() / 100;
        float elapedsSecconds =  GetSecondsSinceLastEncounter(State.lastEncounter);
        return (int)(elapedsSecconds * ( 1 + encounterSpeedModifier) / encounterSpeed);
    }
}
