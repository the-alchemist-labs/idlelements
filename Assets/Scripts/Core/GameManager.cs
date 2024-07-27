using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            gameObject.AddComponent<MapCatalog>();
            gameObject.AddComponent<ElementalCatalog>();
            gameObject.AddComponent<InventoryCatalog>();
            gameObject.AddComponent<SkillCatalog>();
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

        GameEvents.OnPlayerInitialized += StartGame;
    }

    void OnDestroy()
    {
        GameEvents.OnPlayerInitialized -= StartGame;
        Save();
    }

    void StartGame()
    {
        StartCoroutine(Backup());
        SceneManager.LoadScene(SceneNames.Main);
    }

    private IEnumerator Backup()
    {
        while (true)
        {
            Save();
            yield return new WaitForSeconds(5);
        }
    }

    private void Save()
    {
        MapManagerState mms = new MapManagerState(MapManager.Instance.progressions, MapManager.Instance.currentMapId);

        ElementalManagerState ems = new ElementalManagerState(
            ElementalManager.Instance.entries,
            ElementalManager.Instance.lastEncounter,
            ElementalManager.Instance.equipedSkills
        );

        Player p = Player.Instance;
        PlayerState ps = new PlayerState(
            p.Level,
            p.Experience,
            Player.Instance.Party,
            Player.Instance.Resources,
            Player.Instance.Inventory
        );

        DataService.Instance.SaveData(FileName.ElementalManagerState, true, ems);
        DataService.Instance.SaveData(FileName.MapManagerState, true, mms);
        DataService.Instance.SaveData(FileName.PlayerState, true, ps);
    }

}
