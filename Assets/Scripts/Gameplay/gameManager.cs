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

    void Update()
    {
        // HeaderBannerManager.Instance.UpdateGoldText(State.essence.ToString());
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
                Enumerable.Range(0, encounters).ToList().ForEach(_ => TriggerEncounter());
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

    void TriggerEncounter()
    {
        Elemental elemental = Maps.GetMap(State.currentMap).GetEncounter();
        bool isCaught = elemental.IsCaught(); // add modifiers
        if (isCaught)
        {
            State.UpdateEssence(elemental.essenceGain);
            State.GainExperience(elemental.expGain);
            Deck.RegisterElement(elemental.id);
        }
        Debug.Log($"A wild {elemental.name} apperead, it was { (isCaught ? "" : "not")} caught\n Level: {State.level} Exp: {State.experience} Essence: {State.essence}");
    }
}
