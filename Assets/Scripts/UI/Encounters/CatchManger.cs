using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatchManager : MonoBehaviour
{
    [SerializeField] ElementalEncounterPanel encounterPanel;
    [SerializeField] BallsScrollView balls;
    [SerializeField] Button throwButton;

    private Encounter _encounter;
    private BallId _selectedBallId;

    void Start()
    {
        GameEvents.OnBallSelected += UpdateSelectedBall;
        GameEvents.OnEncounterUpdated += OnEncouterUpdated;

        _selectedBallId = (BallId)PlayerPrefs.GetInt(PlayerPrefKeys.SELECTED_BALL, (int)BallId.Normal);
        _encounter = ElementalManager.Instance.lastEncounter ?? new Encounter();

        if (_encounter.EncounterId == ElementalId.None)
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
        bool isCaught = ElementalManager.Instance.CatchElemental(_encounter.EncounterId, _selectedBallId);
        Player.Instance.Inventory.UpdateBalls(_selectedBallId, -1);
        _encounter.UseTry(isCaught);

        if (isCaught)
        {
            // HANDLE
        }
        else
        {
            // HandleFailedCatch();
        }

        GameEvents.EncounterUpdated();
    }

    private void UpdateSelectedBall()
    {
        _selectedBallId = balls.selectedBall;
        Ball selectedBall = InventoryCatalog.Instance.GetBall(balls.selectedBall);
        throwButton.GetComponentInChildren<TMP_Text>().text = $"Catch {selectedBall.CatchRate * 100}%";
        throwButton.interactable = canThrowBall();
    }

    private void OnEncouterUpdated()
    {
        ElementalManager.Instance.UpdatelastEncounter(_encounter);
        throwButton.interactable = canThrowBall();
        encounterPanel.UpdateUI(_encounter);
    }

    private void GetNewEncounter()
    {
        ElementalId elementalId = MapManager.Instance.currentMap.GetElementalEncounter();
        _encounter.SetNewEncounter(elementalId);
        GameEvents.EncounterUpdated();
    }

    private bool canThrowBall()
    {
        bool HasRemainingTries = _selectedBallId != BallId.None && Player.Instance.Inventory.Balls[_selectedBallId] > 0;
        return _encounter.HasRemainingTries() && HasRemainingTries;
    }
}