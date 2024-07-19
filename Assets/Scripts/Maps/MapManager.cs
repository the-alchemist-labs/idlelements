using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public List<MapProgression> progressions { get; private set; }
    public MapId currentMapId { get; private set; }

    public Map currentMap { get { return MapCatalog.Instance.GetMap(currentMapId); } private set { } }
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
        MapManagerState state = DataService.Instance.LoadData<MapManagerState>(FileName.MapManagerState, true);
        progressions = state.progressions;
        currentMapId = state.currentMapId;
    }

    public void UpdateCurrentMap(MapId id)
    {
        currentMapId = id;
        currentMapProgression = GetMapProgression(currentMapId);
        GameEvents.MapDataChanged();
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
}