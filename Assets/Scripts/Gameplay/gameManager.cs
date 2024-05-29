using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        StartCoroutine(ProgressRoutine());
    }

    void OnDestroy()
    {
        State.Save();
    }



    IEnumerator ProgressRoutine()
    {
        while (true)
        {
            int elapedsSecconds = GetSecondsDiff(State.lastEncounter);
            int encounters = elapedsSecconds / encounterRate;
            int remainder = elapedsSecconds % encounterRate;
            if (encounters > 0)
            {
                Enumerable.Range(0, encounters).ToList().ForEach(_ => GameActions.TriggerEncounter());
                State.UpdateLastEncounter(DateTime.Now.AddSeconds(remainder));
            }

            yield return new WaitForSeconds(1);
        }
    }

    int GetSecondsDiff(DateTime date)
    {
        TimeSpan diff = DateTime.Now - date;
        return (int)diff.TotalSeconds;
    }
}
