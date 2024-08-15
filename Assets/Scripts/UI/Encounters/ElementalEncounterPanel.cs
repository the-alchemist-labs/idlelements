using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementalEncounterPanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text tierText;
    
    [SerializeField] TMP_Text catchRate;
    
    [SerializeField] Image encounterImage;
    [SerializeField] Image caughtIndicator;
    [SerializeField] Material backAndWhiteMaterial;
    
    [SerializeField] TMP_Text baseCatchRateText;

    private void Awake()
    {
        GameEvents.OnEncounterUpdated += UpdateUI;
        GameEvents.OnBallSelected += UpdateUI;
    }

    void Start()
    {
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnEncounterUpdated -= UpdateUI;
        GameEvents.OnBallSelected -= UpdateUI;
    }

    public void UpdateUI()
    {
        Encounter encounter = EncounterManger.Instance.Encounter;
        Elemental elemental = ElementalCatalog.Instance.GetElemental(encounter.EncounterId);
        Ball ball = InventoryCatalog.Instance.GetBall(EncounterManger.Instance.SelectedBallId);
        bool isCaught = ElementalManager.Instance.IsElementalRegistered(elemental.Id);
        
        nameText.text = elemental.Name;
        descriptionText.text = elemental.Description;
        typeText.text = $"Type: {elemental.Type}";
        tierText.text = $"Type: {elemental.Tier}";
        baseCatchRateText.text = $"Catch: {ball.GetCatchRate(elemental.Tier) * 100}%";

        encounterImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.Id}");
        caughtIndicator.material = isCaught ? null : backAndWhiteMaterial;
    }
}