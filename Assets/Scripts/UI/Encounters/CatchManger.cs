using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatchManager : MonoBehaviour
{
    [SerializeField] ElementalEncounterPanel encounterPanel;
    [SerializeField] BallsScrollView balls;
    [SerializeField] Button throwButton;

    private Encounter encounter;
    private BallId selectedBallId;

    void Start()
    {
        GameEvents.OnBallSelected += UpdateSelectedBall;
        GameEvents.OnEncounterUpdated += OnEncouterUpdated;

        selectedBallId = (BallId)PlayerPrefs.GetInt(PlayerPrefKeys.SELECTED_BALL, (int)BallId.Normal);
        encounter = ElementalManager.Instance.lastEncounter ?? new Encounter();

        if (encounter.EncounterId == ElementalId.None)
        {
            GetNewEncounter();
            return;
        }

        GameEvents.EncounterUpdated();
    }

    void OnDestroy()
    {
        GameEvents.OnBallSelected -= UpdateSelectedBall;
        GameEvents.OnEncounterUpdated -= OnEncouterUpdated;
    }

    public void ThrowBall()
    {
        bool isCaught = ElementalManager.Instance.CatchElemental(encounter.EncounterId, selectedBallId);
        Player.Instance.Inventory.UpdateBalls(selectedBallId, -1);
        encounter.UseTry(isCaught);

        if (isCaught)
        {
            // HandleSuccessfulCatch();
        }
        else
        {
            // HandleFailedCatch();
        }

        GameEvents.EncounterUpdated();
    }

    private void UpdateSelectedBall()
    {
        selectedBallId = balls.selectedBall;
        Ball selectedBall = InventoryCatalog.Instance.GetBall(balls.selectedBall);
        throwButton.GetComponentInChildren<TMP_Text>().text = $"Catch {selectedBall.CatchRate * 100}%";
        throwButton.interactable = canThrowBall();
    }

    private void OnEncouterUpdated()
    {
        ElementalManager.Instance.UpdatelastEncounter(encounter);
        throwButton.interactable = canThrowBall();
        encounterPanel.UpdateUI(encounter);
    }

    private void GetNewEncounter()
    {
        ElementalId elementalId = MapManager.Instance.currentMap.GetElementalEncounter();
        encounter.SetNewEncounter(elementalId);
        GameEvents.EncounterUpdated();
    }

    private bool canThrowBall()
    {
        bool HasRemainingTries = selectedBallId != BallId.None && Player.Instance.Inventory.Balls[selectedBallId] > 0;
        return encounter.HasRemainingTries() && HasRemainingTries;
    }
}