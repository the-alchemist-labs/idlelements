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

            gameObject.AddComponent<MapCatalog>();
            gameObject.AddComponent<ElementalCatalog>();
            gameObject.AddComponent<MapManager>();
            gameObject.AddComponent<ElementalManager>();
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
        while (!IsReady())
        {
            yield return null;
        }

        afkGainsPanel.GetComponent<AfkGainsPanel>()?.DisplayAfkGains();
        StartCoroutine(Temple.StartRoutine());
        StartCoroutine(Backup());
    }

    private void Save()
    {
        MapManagerState mms = new MapManagerState(MapManager.Instance.progressions, MapManager.Instance.currentMapId);

        ElementalManagerState ems = new ElementalManagerState(
            ElementalManager.Instance.entries,
            ElementalManager.Instance.lastCaught,
            ElementalManager.Instance.lastEncounterDate
        );

        Player p = Player.Instance;
        PlayerState ps = new PlayerState(
            p.Level,
            p.Experience,
            new Party(p.Party.First, p.Party.Second, p.Party.Third),
            new PlayerResources(p.Resources.Essence, p.Resources.Gold, p.Resources.Orbs)
        );

        DataService.Instance.SaveData(FileName.ElementalManagerState, true, ems);
        DataService.Instance.SaveData(FileName.MapManagerState, true, mms);
        DataService.Instance.SaveData(FileName.PlayerState, true, ps);
    }

    private IEnumerator Backup()
    {
        Save();
        yield return new WaitForSeconds(1);
    }

    public bool IsReady()
    {
        return
            Player.Instance.Id != null 
            && ElementalManager.Instance?.lastEncounterDate.Year != 1
            && Player.Instance.Party != null;
    }
}
