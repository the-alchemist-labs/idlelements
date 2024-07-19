using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCatalog : MonoBehaviour
{
    public static MapCatalog Instance { get; private set; }
    public List<Map> maps { get; private set; }

    private void Awake()
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

    private void Initialize()
    {
        maps = DataService.Instance.LoadData<List<Map>>(FileName.MapCatalog, false);
    }

    public Map GetMap(MapId id)
    {
        return maps.Find(el => el.id == id);
    }


    public Map GetUnlockedMapByLevel(int level)
    {
        return maps.Where(m => m.requiredLevel == level).FirstOrDefault();
    }
}