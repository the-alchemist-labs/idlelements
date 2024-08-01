using System.Collections.Generic;
using UnityEngine;

public class ElementalCatalog : MonoBehaviour
{
    public static ElementalCatalog Instance { get; private set; }
    public List<Elemental> Elementals { get; private set; }
    public List<Minimental> Minimentals { get; private set; }
    public List<ElementalSkill> Skills { get; private set; }
    public int Count { get { return Elementals.Count; } }

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
        Elementals = DataService.Instance.LoadData<List<Elemental>>(FileName.ElementalCatalog, false);
        Minimentals = DataService.Instance.LoadData<List<Minimental>>(FileName.MinimentalCatalog, false);
        Skills = DataService.Instance.LoadData<List<ElementalSkill>>(FileName.SkillCatalog, false);
    }

    public Elemental GetElemental(ElementalId id)
    {
        return Elementals.Find(el => el.id == id);
    }

    public Minimental GetElemental(MinimentalId id)
    {
        return Minimentals.Find(el => el.id == id);
    }

    public ElementalSkill GetSkill(SkillId id)
    {
        return Skills.Find(el => el.Id == id);
    }

    
    public int CalculateDPS(Elemental elemental, int level)
    {
        return elemental.Stats.Attack * level;
    }

    public int CalculateEHP(Elemental elemental, int level)
    {
        return (elemental.Stats.Hp * level) + (elemental.Stats.Defense * 10);

    }
}
