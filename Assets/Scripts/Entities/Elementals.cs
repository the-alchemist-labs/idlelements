using System.Collections.Generic;

public class Elementals
{
    public static List<Elemental> all { get; private set; }

    static Elementals()
    {
        all = DataService.Instance.LoadData<List<Elemental>>("elementals");
    }

    public static Elemental GetElement(ElementalId id) {
        return all.Find(el => el.id == id);
    }
}