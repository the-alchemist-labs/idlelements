using System;
using System.Collections.Generic;
using Encounters;
using UnityEngine;

public class EncounterManger : MonoBehaviour
{
    public static EncounterManger Instance;

    public List<CatchTryCost> CatchTryCost;
    public Encounter Encounter;
    public BallId SelectedBallId;
    public int RerollCost;
    
    [SerializeField] BallsScrollView balls;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            GameEvents.OnEncounterUpdated += OnEncounterUpdated;
            SelectedBallId = (BallId)PlayerPrefs.GetInt(PlayerPrefKeys.SELECTED_BALL, (int)BallId.NormalBall);

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Encounter = ElementalManager.Instance.lastEncounter ?? new Encounter();

        if (Encounter.EncounterId == ElementalId.None)
        {
            GetNewEncounter();
            return;
        }

        GameEvents.EncounterUpdated();
    }

    void OnDestroy()
    {
        GameEvents.OnEncounterUpdated -= OnEncounterUpdated;
    }

    public void GetNewEncounter()
    {
        if (Player.Instance.Resources.Essence >= RerollCost)
        {
            Player.Instance.Resources.UpdateEssence(-RerollCost);
        }
        else
        {
            return;
        }
        
        ElementalId elementalId = MapManager.Instance.currentMap.GetElementalEncounter();
        Encounter.SetNewEncounter(elementalId);
        GameEvents.EncounterUpdated();
    }

    public void UpdateSelectedBall(BallId ballId)
    {
        SelectedBallId = ballId;
        PlayerPrefs.SetInt(PlayerPrefKeys.SELECTED_BALL, (int)ballId);
        GameEvents.BallSelected();
    }
    
    private void OnEncounterUpdated()
    {
        ElementalManager.Instance.UpdateLastEncounter(Encounter);
    }
}
