using UnityEngine;

public enum SkillId
{
    None,
    Default,
    FireBall,
    WaterBall
}


public enum AttackTarget
{
    Single,
    AoE,
    Self,
}

public enum TravelTime
{
    Instant = 0,
    Slow = 3,
    Normal = 5,
    Fast = 7,
    VeryFast = 10
}

[CreateAssetMenu(menuName = "Scriptable Objects/Skills")]
public class Skill: ScriptableObject
{
    public SkillId Id;
    public string Name;
    public string Description;
    public ElementType Type;
    public AttackTarget AttackTarget;
    public int ImpactValue;
    public TravelTime SkillSpeed;
}
