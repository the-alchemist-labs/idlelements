using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RerollEncounterButton : MonoBehaviour
{
    [SerializeField] Button rerollButton;
    [SerializeField] TMP_Text rerollCostText;    
    void Awake()
    {
        GameEvents.OnEssenceUpdated += UpdateRerollButton;
    }

    void Start()
    {
        UpdateRerollButton();
    }
    void OnDestroy()
    {
        GameEvents.OnEssenceUpdated -= UpdateRerollButton;
    }

    public void RerollEncounter()
    {
        // sound
        EncounterManger.Instance.GetNewEncounter();
    }
    
    private void UpdateRerollButton()
    {
        int rerollCost = EncounterManger.Instance.RerollCost;
        rerollCostText.text = $"{rerollCost}";
        rerollButton.interactable = Player.Instance.Resources.Essence >= rerollCost;
    }
}
