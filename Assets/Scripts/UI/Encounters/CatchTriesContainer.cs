using System;
using System.Collections.Generic;
using System.Linq;
using Encounters;
using UnityEngine;

public class CatchTriesContainer : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject tryPrefab;

    private void Awake()
    {
        GameEvents.OnEncounterUpdated += UpdateUI;
    }

    private void OnDestroy()
    {
        GameEvents.OnEncounterUpdated -= UpdateUI;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        List<CatchTryCost> catchTryCosts = EncounterManger.Instance.CatchTryCost;

        container.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        for (int i = 0; i < catchTryCosts.Count; i++)
        {
            Encounter encounter = EncounterManger.Instance.Encounter;
            CatchTryCost tryCost = catchTryCosts[i];
            GameObject newTry = Instantiate(tryPrefab, container);

            if (newTry.TryGetComponent(out CatchTryPrefab item))
            {
                item.Init(i, tryCost, i == encounter.Tries, encounter.Tries > i);
            }
        }
    }
}