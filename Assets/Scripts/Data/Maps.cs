using System.Collections.Generic;

public class MapsData
{
    public List<Map> all { get; private set; }
    public List<MapProgression> progressions { get; private set; }

    public MapsData(List<Map> maps, List<MapProgression> mapsProgression)
    {
        all = maps;
        progressions = mapsProgression ?? new List<MapProgression>();
    }

    public Map GetMap(MapId id)
    {
        return all.Find(el => el.id == id);
    }

    public void UpdateMapProgression(int catches)
    {
        GetMapProgression(State.currentMap).catchProgression += catches;
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