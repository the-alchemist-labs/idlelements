
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
    public List<Reward> Rewards;

    ElementalStats IElemental.Stats => Stats;
    List<SkillByLevel> IElemental.Skills => Skills;
    List<Reward> IElemental.Rewards => Rewards;
}