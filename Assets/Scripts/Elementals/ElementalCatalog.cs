using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementalCatalog : MonoBehaviour
{
    private const string SCRIPTABLE_OBJECT_ELEMENTALS_PATH = "ScriptableObjects/Elementals";
    private const string SCRIPTABLE_OBJECT_MINIMENTALS_PATH = "ScriptableObjects/Minimentals";
    private const string SCRIPTABLE_OBJECT_SKILLS_PATH = "ScriptableObjects/Skills";

    public static ElementalCatalog Instance { get; private set; }
    public List<Elemental> Elementals
    {
        get => _elementals.Where(s => s.Id != ElementalId.None).ToList();
    }
    public List<Minimental> Minimentals { get; private set; }
    public List<Skill> Skills { get; private set; }
    public int Count { get { return Elementals.Count; } }

    private List<Elemental> _elementals;
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
        _elementals = Resources.LoadAll<Elemental>(SCRIPTABLE_OBJECT_ELEMENTALS_PATH).ToList();
        Minimentals = Resources.LoadAll<Minimental>(SCRIPTABLE_OBJECT_MINIMENTALS_PATH).ToList();
        Skills = Resources.LoadAll<Skill>(SCRIPTABLE_OBJECT_SKILLS_PATH).ToList();
    }

    public Elemental GetElemental(ElementalId id)
    {
        return Elementals.Find(el => el.Id == id);
    }

    public Minimental GetElemental(MinimentalId id)
    {
        return Minimentals.Find(el => el.Id == id);
    }

    public Skill GetSkill(SkillId id)
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
