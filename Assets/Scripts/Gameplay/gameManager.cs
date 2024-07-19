using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject afkGainsPanel;
    public GameObject playerInfoPanel;
    public GameState state { get; private set; }

    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            state = DataService.Instance.LoadData<GameState>(FileName.State, true);

            gameObject.AddComponent<MapsData>();
            gameObject.AddComponent<ElementalsData>();
            gameObject.AddComponent<ResourcesData>();
            gameObject.AddComponent<MainThreadDispatcher>();
            gameObject.AddComponent<Player>();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;

        StartCoroutine(StartGame());

    }

    void OnDestroy()
    {
        Save();
    }

    IEnumerator StartGame()
    {
        while (Player.Instance.Id == null)
        {
            yield return null;
        }

        afkGainsPanel.GetComponent<AfkGainsPanel>()?.DisplayAfkGains();
        StartCoroutine(Temple.StartRoutine());
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
