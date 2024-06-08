using System.Collections.Generic;

public class MapsData
{
    public List<Map> all { get; private set; }
    public List<MapProgression> progressions { get; private set; }
    public MapId currentMapId { get; private set; }
    public Map currentMap { get; private set; }
    public MapProgression currentMapProgression { get; private set; }

    public void UpdateCurrentMap(MapId id)
    {
        currentMapId = id;
        currentMap = all.Find(el => el.id == currentMapId);
        currentMapProgression = GetMapProgression();
    }

    public MapsData(List<Map> maps, List<MapProgression> mapsProgression, MapId currentMapId)
    {
        all = maps;
        progressions = mapsProgression ?? new List<MapProgression>();
        this.currentMapId = currentMapId == 0 ? MapId.MapA : currentMapId;
        currentMap = all.Find(el => el.id == currentMapId);
        currentMapProgression = GetMapProgression();
    }

    public MapProgression GetCurrentMapProgresion()
    {
        return progressions.Find(el => el.id == currentMapId);
    }

    public MapProgression GetMapProgresion(MapId id)
    {
        return progressions.Find(el => el.id == id);
    }

    private MapProgression GetMapProgression()
    {
        MapProgression mapProgression = progressions.Find(m => m.id == currentMapId);
        if (mapProgression == null)
        {
            mapProgression = new MapProgression() { id = currentMapId };
            progressions.Add(mapProgression);
        }

        return mapProgression;
    }
}