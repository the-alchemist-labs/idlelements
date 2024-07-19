using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapsData : MonoBehaviour
{
    public static MapsData Instance { get; private set; }

    public List<Map> all { get; private set; }
    public List<MapProgression> progressions { get; private set; }
    public MapId currentMapId { get; private set; }

    public Map currentMap { get { return all.Find(el => el.id == currentMapId); } private set { } }
    public MapProgression currentMapProgression { get { return progressions.Find(map => map.id == currentMapId); } private set { } }

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
        GameState gs = DataService.Instance.LoadData<GameState>(FileName.State, true);
        all = DataService.Instance.LoadData<List<Map>>(FileName.Maps, false);

        progressions = gs.mapsProgression ?? new List<MapProgression>();
        currentMapId = gs.currentMapId == 0 ? MapId.FireWater : gs.currentMapId;
    }

    public void UpdateCurrentMap(MapId id)
    {
        currentMapId = id;
        currentMapProgression = GetMapProgression(currentMapId);
        GameEvents.MapDataChanged();
    }

    public Map GetMap(MapId id)
    {
        return all.Find(el => el.id == id);
    }

    public MapProgression GetMapProgression(MapId id)
    {
        MapProgression mapProgression = progressions.Find(m => m.id == id);
        if (mapProgression == null)
        {
            mapProgression = new MapProgression() { id = id };
            progressions.Add(mapProgression);
        }

        return mapProgression;
    }

    public Map GetUnlockedMapByLevel(int level)
    {
        return all.Where(m => m.requiredLevel == level).FirstOrDefault();
    }
}