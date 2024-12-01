using Encounters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThrowBallButton : MonoBehaviour
{
    [SerializeField] private Button throwButton;
    [SerializeField] private HorizontalLayoutGroup layout;
    [SerializeField] private Image currencyImage;
    [SerializeField] private TMP_Text costText;

    void Awake()
    {
        GameEvents.OnBallSelected += UpdateUI;
        GameEvents.OnEssenceUpdated += UpdateUI;
        GameEvents.OnEncounterUpdated += UpdateUI;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        GameEvents.OnEncounterUpdated -= UpdateUI;
        GameEvents.OnBallSelected -= UpdateUI;
        GameEvents.OnEssenceUpdated -= UpdateUI;
    }

    private void UpdateUI()
    {
        int encounterTry = EncounterManger.Instance.Encounter.Tries;
        CatchTryCost catchTryCost = EncounterManger.Instance.CatchTryCost[encounterTry];
        currencyImage.gameObject.SetActive(catchTryCost.Currency != Resource.None);
        currencyImage.sprite = Resources.Load<Sprite>($"Sprites/Currencies/{catchTryCost.Currency}");
        costText.text = costText.text = catchTryCost.Cost == 0 ? "Free" : $"{catchTryCost.Cost}";

        LayoutRebuilder.ForceRebuildLayoutImmediate(costText.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());

        throwButton.interactable = CanThrowBall();
    }

    public void ThrowBall()
    {
        BallId selectedBallId = EncounterManger.Instance.SelectedBallId;
        Encounter encounter = EncounterManger.Instance.Encounter;
        
        UseResourcesToThrow(selectedBallId);
        bool isCaught = ElementalManager.Instance.CatchElemental(encounter.EncounterId, selectedBallId);
        
        encounter.UseTry(isCaught);

        if (isCaught)
        {
            SoundManager.Instance.PlaySystemSFX(SystemSFXId.Celebration);
            EncounterManger.Instance.EncounterCaught();
        }
        else
        {
            SoundManager.Instance.PlaySystemSFX(SystemSFXId.Failed);
        }

        GameEvents.EncounterUpdated();
    }

    private bool CanThrowBall()
    {
        BallId selectedBallId = EncounterManger.Instance.SelectedBallId;
        bool hasRemainingTries = selectedBallId != BallId.None && Player.Instance.Inventory.Balls[selectedBallId] > 0;
        return EncounterManger.Instance.Encounter.HasRemainingTries() && hasRemainingTries && EnoughResourcesToThrow();
    }

    private bool EnoughResourcesToThrow()
    {
        CatchTryCost catchTryCost = EncounterManger.Instance.CatchTryCost[EncounterManger.Instance.Encounter.Tries];
        switch (catchTryCost.Currency)
        {
            case Resource.None:
                return true;
            case Resource.Essence:
                return Player.Instance.Resources.Essence >= catchTryCost.Cost;
            case Resource.Orb:
                return Player.Instance.Resources.Orbs >= catchTryCost.Cost;
        }

        return false;
    }

    private void UseResourcesToThrow(BallId selectedBallId)
    {
        CatchTryCost catchTryCost = EncounterManger.Instance.CatchTryCost[EncounterManger.Instance.Encounter.Tries];
        switch (catchTryCost.Currency)
        {
            case Resource.Essence:
                Player.Instance.Resources.UpdateEssence(-catchTryCost.Cost);
                break;
            case Resource.Orb:
                Player.Instance.Resources.UpdateOrbs(-catchTryCost.Cost);
                break;
        }
        Player.Instance.Inventory.UpdateBalls(selectedBallId, -1);

    }
}