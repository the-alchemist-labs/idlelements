using System.Collections;
using UnityEngine;

public class State : MonoBehaviour
{
    public static State Instance { get; private set; }
    public GameState state { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        Save();
    }

    private void Initialize()
    {
        state = DataService.Instance.LoadData<GameState>(FileName.State, true);

        gameObject.AddComponent<MapsData>();
        gameObject.AddComponent<ElementalsData>();
        gameObject.AddComponent<ResourcesData>();

        StartCoroutine(Backup());
    }

    private void Save()
    {
        GameState gs = new GameState()
        {
            lastEncounterDate = ElementalsData.Instance.lastEncounterDate,
            currentMapId = MapsData.Instance.currentMapId,
            level = Player.Instance.Level,
            experience = Player.Instance.Experience,
            essence = ResourcesData.Instance.Essence,
            gold = ResourcesData.Instance.Gold,
            orbs = ResourcesData.Instance.Orbs,
            elementalEnteries = ElementalsData.Instance.entries,
            mapsProgression = MapsData.Instance.progressions,
            party = Player.Instance.Party,
            lastCaught = ElementalsData.Instance.lastCaught,
        };

        DataService.Instance.SaveData(FileName.State, true, gs);
    }

    private IEnumerator Backup()
    {
        Save();
        yield return new WaitForSeconds(1);
    }
}