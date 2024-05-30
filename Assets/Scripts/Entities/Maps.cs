using System.Collections.Generic;

public static class Maps
{
    public static List<Map> all { get; private set; }

    static Maps()
    {
        all = DataService.Instance.LoadData<List<Map>>(FileName.Maps);
    }

    public static Map GetMap(MapId id) {
        return all.Find(el => el.id == id);
    }
}