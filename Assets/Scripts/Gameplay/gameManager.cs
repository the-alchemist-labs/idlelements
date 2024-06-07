using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject afkGainsPanel;
    private static GameManager instance;

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
        StartCoroutine(Backup());
    }

    void OnDestroy()
    {
        State.Save();
    }

    void HandleAfkGainsPanel()
    {
        int encounters = GetEncounters();
        int delta = GetEncounterDeltaTime(encounters);

        IdleRewards rewards = GameActions.TriggerMultipleEncounters(encounters);
        State.UpdateLastEncounter(DateTime.Now.AddSeconds(-delta));

        StartCoroutine(ProgressRoutine());
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
            int delta = GetEncounterDeltaTime(encounters);

            if (encounters > 0)
            {
                IdleRewards rewards = encounters == 1
                ? GameActions.TriggerEncounter()
                : GameActions.TriggerMultipleEncounters(encounters);
                GameActions.EarnRewardOfEncounters(rewards);
                State.UpdateLastEncounter(DateTime.Now.AddSeconds(-delta));
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
        float elapedsSecconds = GetSecondsSinceLastEncounter(State.lastEncounter);
        return (int)(elapedsSecconds / State.GetEncounterSpeed());
    }

    int GetEncounterDeltaTime(int encounters)
    {
        float elapedsSecconds = GetSecondsSinceLastEncounter(State.lastEncounter);
        int encountersTime = encounters * State.GetEncounterSpeed();
        return (int)elapedsSecconds - encountersTime;
    }

}
