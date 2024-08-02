using System;
using System.Collections.Generic;
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

[CreateAssetMenu(fileName = "New Elemental", menuName = "Scriptable Objects/Elementals")]
public class Elemental : ScriptableObject, IElemental
{
    public ElementalId Id;
    public string Name;
    public ElementType Type;
    public float CatchRate;
    public Evolution Evolution;
    public ElementalStats Stats;
    public List<SkillByLevel> Skills;
    public IdleBonus IdleBonus;
    public Rewards Rewards;

    ElementalStats IElemental.Stats => Stats;
    List<SkillByLevel> IElemental.Skills => Skills;
    Rewards IElemental.Rewards => Rewards;
}

[Serializable]
public class IdleBonus
{
    public BonusResource resource;
    public float amount;
}

[Serializable]
public class Evolution
{
    public ElementalId evolveTo;
    public int tokensCost;
    public int essenceCost;
}