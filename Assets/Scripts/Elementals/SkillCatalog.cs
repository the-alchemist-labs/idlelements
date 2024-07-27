using System.Collections.Generic;
using UnityEngine;

public class SkillCatalog : MonoBehaviour
{
    public static SkillCatalog Instance { get; private set; }
    public List<ElementalSkill> Skills { get; private set; }

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
        Skills = DataService.Instance.LoadData<List<ElementalSkill>>(FileName.SkillCatalog, false);
    }

    public ElementalSkill GetSkill(SkillId id)
    {
        return Skills.Find(el => el.Id == id);
    }
}