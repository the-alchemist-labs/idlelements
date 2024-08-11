using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ElementalId
{
    None,
    Ferine,
    Ferion,
    Wizo,
    Wizar,
    Bolli,
    Bider,
    Bulldo,
    Seria,
    Galeria,
    Freezion,
    Stromeon,
    Zapeon,
    Volx,
    Serphire,
}

public enum Tier
{
    Common,
    Rare,
    Epic,
    Unique,
    Legendary
}

[CreateAssetMenu(fileName = "New Elemental", menuName = "Scriptable Objects/Elementals")]
public class Elemental : ScriptableObject, IElemental
{
    public ElementalId Id;
    public string Name;
    public string Description;
    public ElementType Type;
    public Tier Tier;
    public Evolution Evolution;
    public ElementalStats Stats;
    [SerializeField] List<SkillByLevel> skills;
    public Rewards Rewards;

    public List<SkillByLevel> Skills => skills.OrderBy(s => s.Level)
        .Where(s => s.SkillId != SkillId.None)
        .ToList();

    ElementalStats IElemental.Stats => Stats;
    List<SkillByLevel> IElemental.Skills => Skills;
    Rewards IElemental.Rewards => Rewards;
}

[Serializable]
public class Evolution
{
    public ElementalId evolveTo;
    public int tokensCost;
    public int essenceCost;
}