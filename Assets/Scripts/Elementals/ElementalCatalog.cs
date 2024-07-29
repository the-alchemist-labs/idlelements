using System.Collections.Generic;
using UnityEngine;

public class ElementalCatalog : MonoBehaviour
{
    public static ElementalCatalog Instance { get; private set; }
    public List<Elemental> elementals { get; private set; }
    public List<Minimental> minimentals { get; private set; }
    public int Count { get { return elementals.Count; } }

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
        elementals = DataService.Instance.LoadData<List<Elemental>>(FileName.ElementalCatalog, false);
        minimentals = DataService.Instance.LoadData<List<Minimental>>(FileName.MinimentalCatalog, false);
    }

    public Elemental GetElemental(ElementalId id)
    {
        return elementals.Find(el => el.id == id);
    }

    public Minimental GetElemental(MinimentalId id)
    {
        return minimentals.Find(el => el.id == id);
    }
}