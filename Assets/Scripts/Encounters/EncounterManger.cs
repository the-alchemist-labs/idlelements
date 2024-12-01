using System;
using System.Collections.Generic;
using Encounters;
using TMPro;
using UnityEngine;

public class EncounterManger : MonoBehaviour
{
    public static EncounterManger Instance;

    public List<CatchTryCost> CatchTryCost;
    public Encounter Encounter;
    public BallId SelectedBallId;
    public int RerollCost;
    
    [SerializeField] private TMP_Text RerollCostText;
    [SerializeField] private BallsScrollView balls;
    
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
        if (Player.Instance.Resources.Essence >= RerollCost || Encounter.State == EncounterState.Caught)
        {
            Player.Instance.Resources.UpdateEssence(-RerollCost);
            RerollCostText.text = RerollCost.ToString();
        }
        else
        {
            return;
        }
        
        ElementalId elementalId = MapManager.Instance.currentMap.GetElementalEncounter();
        Encounter.SetNewEncounter(elementalId);
        GameEvents.EncounterUpdated();
        DailyEvents.NewEncounter();
    }

    public void UpdateSelectedBall(BallId ballId)
    {
        SelectedBallId = ballId;
        PlayerPrefs.SetInt(PlayerPrefKeys.SELECTED_BALL, (int)ballId);
        GameEvents.BallSelected();
    }

    public void EncounterCaught()
    {
        RerollCostText.text = "Free";
    }
    private void OnEncounterUpdated()
    {
        ElementalManager.Instance.UpdateLastEncounter(Encounter);
    }
}
