using System.Collections.Generic;

public class MapsData
{
    public List<Map> all { get; private set; }
    public List<MapProgression> progressions { get; private set; }
    public MapId currentMapId { get; private set; }
    public Map currentMap { get; private set; }
    public MapProgression currentMapProgression { get { return progressions.Find(el => el.id == currentMapId); } private set {}}

    public Map GetMap(MapId id)
    {
        return all.Find(el => el.id == id);
    }
    
    public void UpdateCurrentMap(MapId id)
    {
        currentMapId = id;
        currentMap = all.Find(el => el.id == currentMapId);
        currentMapProgression = GetMapProgression(currentMapId);
        GameEvents.MapDataChanged();
    }

    public MapsData(List<Map> maps, List<MapProgression> mapsProgression, MapId currentMapId)
    {
        all = maps;
        progressions = mapsProgression ?? new List<MapProgression>();
        this.currentMapId = currentMapId == 0 ? MapId.FireWater : currentMapId;
        currentMap = all.Find(el => el.id == currentMapId);
        currentMapProgression = GetMapProgression(currentMapId);
    }

    public MapProgression GetCurrentMapProgresion()
    {
        return progressions.Find(el => el.id == currentMapId);
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