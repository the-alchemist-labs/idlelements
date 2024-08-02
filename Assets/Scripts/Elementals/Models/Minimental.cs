
using System.Collections.Generic;
using UnityEngine;

public enum MinimentalId
{
    None,
    FireMeele,
    FireRanged,
    FireBoss,
}

[CreateAssetMenu(menuName = "Scriptable Objects/Minimentals")]
public class Minimental : ScriptableObject, IElemental
{
    public MinimentalId Id;
    public string Name;
    public ElementType Type;
    public ElementalStats Stats;
    public List<SkillByLevel> Skills;
    public Rewards Rewards;

    ElementalStats IElemental.Stats => Stats;
    List<SkillByLevel> IElemental.Skills => Skills;
    Rewards IElemental.Rewards => Rewards;
}