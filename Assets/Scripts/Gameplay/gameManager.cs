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
        float elapedsSeconds = GameActions.GetSecondsSinceLastEncounter(State.lastEncounter);
        string idleTimeString = TextUtil.FormatSecondsToTimeString(elapedsSeconds);

        int encounters = GameActions.GetEncounters(elapedsSeconds);
        
        int delta = GetEncounterDeltaTime(encounters);

        IdleRewards rewards = GameActions.RunEncounters(elapedsSeconds);
        State.UpdateLastEncounter(DateTime.Now.AddSeconds(-delta));

        StartCoroutine(ProgressRoutine()); 
        if (afkGainsPanel.TryGetComponent(out AfkGainsPanel panel))
        {
            panel.DisaplyAfkGains(rewards, idleTimeString);
        }
    }

    IEnumerator ProgressRoutine()
    {
        while (true)
        {
            float elapedsSeconds = GameActions.GetSecondsSinceLastEncounter(State.lastEncounter);
            int encounters = GameActions.GetEncounters(elapedsSeconds);
            int delta = GetEncounterDeltaTime(encounters);

            if (encounters > 0)
            {
                IdleRewards rewards = GameActions.RunEncounters(elapedsSeconds);
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

    int GetEncounterDeltaTime(int encounters)
    {
        float elapedsSeconds = GameActions.GetSecondsSinceLastEncounter(State.lastEncounter);
        int encountersTime = encounters * State.GetEncounterSpeed();
        return (int)elapedsSeconds - encountersTime;
    }

}
