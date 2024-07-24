using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatchManager : MonoBehaviour
{

    [SerializeField] ElementalEncounterPanel encounterPanel;
    [SerializeField] BallsScrollView balls;
    [SerializeField] Button throwButton;

    private Encounter encounter;
    private Elemental elementalEncounter;
    private Ball selectedBall;

    void Start()
    {
        GameEvents.OnBallSelected += UpdateSelectedBall;
        BallId selectedBallId = (BallId)PlayerPrefs.GetInt(PlayerPrefKeys.SELECTED_BALL, (int)BallId.Normal);
        selectedBall = InventoryCatalog.Instance.GetBall(selectedBallId);
        encounter = ElementalManager.Instance.lastEncounter ?? new Encounter();

        if (encounter.EncounterId == ElementalId.None)
        {
            GetNewEncounter();
            return;
        }

        elementalEncounter = ElementalCatalog.Instance.GetElemental(encounter.EncounterId);
        encounterPanel.UpdateUI(elementalEncounter);
    }

    void OnDestroy()
    {
        GameEvents.OnBallSelected -= UpdateSelectedBall;
    }

    private void UpdateSelectedBall()
    {
        selectedBall = InventoryCatalog.Instance.GetBall(balls.selectedBall);
        throwButton.GetComponentInChildren<TMP_Text>().text = $"Catch {selectedBall.CatchRate * 100}%";
        PlayerPrefs.SetInt(PlayerPrefKeys.SELECTED_BALL, (int)selectedBall.Id);
    }


    public void GetNewEncounter()
    {
        ElementalId elementalId = MapManager.Instance.currentMap.GetElementalEncounter();
        encounter.SetNewEncounter(elementalId);
        elementalEncounter = ElementalCatalog.Instance.GetElemental(encounter.EncounterId);
        ElementalManager.Instance.UpdatelastEncounter(encounter);
        throwButton.interactable = encounter.HasRemainingTries();
        encounterPanel.UpdateUI(elementalEncounter);
    }

    public void ThrowBall()
    {
        bool isCaught = ElementalManager.Instance.CatchElemental(elementalEncounter, selectedBall);
        Player.Instance.Inventory.UpdateBalls(selectedBall.Id, -1);

        if (isCaught)
        {
            HandleSuccessfulCatch();
        }
        else
        {
            HandleFailedCatch();
        }

        throwButton.interactable = encounter.HasRemainingTries();
    }

    private void HandleSuccessfulCatch()
    {
        GetNewEncounter();
        Debug.Log("Success");

    }

    private void HandleFailedCatch()
    {
        encounter.UseTry();
        ElementalManager.Instance.UpdatelastEncounter(encounter);
        Debug.Log($"Failed, tries: {encounter.Tries}");
    }
}