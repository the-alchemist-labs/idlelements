#nullable enable
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;

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

public enum MinimentalId
{
    None,
    FireMeele,
    FireRanged,
    FireBoss,
}

public enum ElementalStat
{
    Hp,
    Attack,
    Defense,
    MovmentSpeed,
}

public enum AttackTarget
{
    Single,
    AoE,
    Self,
}

public enum SkillId
{
    Default,
    FireBall,
    WaterBall
}

public enum EncounterState
{
    InProgress,
    Caught,
    OutOfTries,
}

public enum TravelTime
{
    Instant = 0,
    Slow = 3,
    Normal = 5,
    Fast = 7,
    VeryFast = 10
}

[Serializable]
public class Evolution
{
    public ElementalId evolveTo { get; }
    public int tokensCost { get; }
    public int essenceCost { get; }

    public Evolution(ElementalId evolveTo, int tokensCost, int essenceCost)
    {
        this.evolveTo = evolveTo;
        this.tokensCost = tokensCost;
        this.essenceCost = essenceCost;
    }
}

[Serializable]
public class IdleBonus
{
    public BonusResource resource { get; }
    public float amount { get; }

    public IdleBonus(BonusResource resource, float amount)
    {
        this.resource = resource;
        this.amount = amount;
    }
}

public class ElementalSkill
{
    public SkillId Id { get; }
    public string Name { get; }
    public string Description { get; }
    public AttackTarget AttackTarget { get; }
    public ElementalStat TargetedStat { get; }
    public int ImpactValue { get; }
    public TravelTime SkillSpeed { get; }

    public ElementalSkill(SkillId Id, string Name, string Description, AttackTarget AttackTarget, ElementalStat TargetedStat, int ImpactValue, TravelTime SkillSpeed)
    {
        this.Id = Id;
        this.Name = Name;
        this.Description = Description;
        this.AttackTarget = AttackTarget;
        this.TargetedStat = TargetedStat;
        this.ImpactValue = ImpactValue;
        this.SkillSpeed = SkillSpeed;
    }
}

[Serializable]
public class ElementalStats
{
    public int Hp { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int MovmentSpeed { get; private set; }

    private readonly Dictionary<ElementalStat, Action<int>> _statModifiers;

    public ElementalStats(int Hp = 10, int Attack = 1, int Defense = 1, int MovmentSpeed = 1)
    {
        this.Hp = Hp;
        this.Attack = Attack;
        this.Defense = Defense;
        this.MovmentSpeed = MovmentSpeed;

        _statModifiers = new Dictionary<ElementalStat, Action<int>>
        {
            { ElementalStat.Hp, value => Hp += value },
            { ElementalStat.Attack, value => Attack += value },
            { ElementalStat.Defense, value => Defense += value },
            { ElementalStat.MovmentSpeed, value => MovmentSpeed += value }
        };
    }

    public void ModifyStat(ElementalStat stat, int modifyBy)
    {
        if (_statModifiers.TryGetValue(stat, out Action<int> modifyAction))
        {
            modifyAction(modifyBy);
        }
    }
}

[Serializable]
public class RewardsItem
{
    public int Amount { get; }
    public float Chance { get; }

    public RewardsItem(int Amount = 0, float Chance = 0)
    {
        this.Amount = Amount;
        this.Chance = Chance;
    }
}

[Serializable]
public class Rewards
{
    public int Gold { get; }
    public int Essence { get; }
    public int Orbs { get; }
    public int Exp { get; }
    public int Token { get; }
    public Dictionary<BallId, RewardsItem> Balls { get; }

    public Rewards(int Gold = 0, int Essence = 0, int Orbs = 0, int Exp = 0, int Token = 0, Dictionary<BallId, RewardsItem>? Balls = null)
    {
        this.Gold = Gold;
        this.Essence = Essence;
        this.Orbs = Orbs;
        this.Exp = Exp;
        this.Token = Token;
        this.Balls = Balls ?? new Dictionary<BallId, RewardsItem>();
    }
}

public interface IElemental
{
    public string name { get; }
    public ElementType type { get; }
    public ElementalStats Stats { get; }
    public List<SkillId> Skills { get; }
    public Rewards Rewards { get; }
}

[Serializable]
public class Elemental : IElemental
{
    public ElementalId id { get; }
    public string name { get; }
    public ElementType type { get; }
    public float catchRate { get; }
    public Evolution? evolution { get; }
    public ElementalStats Stats { get; }
    public List<SkillId> Skills { get; }
    public int expGain { get; }
    public int orbsGain { get; }
    public IdleBonus? idleBonus { get; }
    public Rewards Rewards { get; }

    public Elemental(
        ElementalId id,
        string name,
        float catchRate,
        Evolution evolution,
        ElementType type,
        ElementalStats Stats,
        List<SkillId> Skills,
        int expGain,
        int orbsGain,
        IdleBonus idleBonus,
        Rewards Rewards
        )
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.catchRate = catchRate;
        this.Stats = new ElementalStats(); // for now
        this.Skills = Skills;
        this.expGain = expGain;
        this.orbsGain = orbsGain;
        this.evolution = evolution;
        this.idleBonus = idleBonus;
        this.Rewards = Rewards ?? new Rewards();
    }
}

[Serializable]
public class Minimental : IElemental
{
    public MinimentalId id { get; }
    public string name { get; }
    public ElementType type { get; }
    public ElementalStats Stats { get; }
    public List<SkillId> Skills { get; }
    public Rewards Rewards { get; }

    public Minimental(
        MinimentalId id,
        string name,
        ElementType type,
        List<SkillId> Skills,
        ElementalStats Stats,
        Rewards Rewards
        )
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.Skills = Skills;
        this.Stats = new ElementalStats(); // for now
        this.Rewards = Rewards ?? new Rewards();
    }
}

[Serializable]
public class ElementalEntry
{
    public ElementalId id;
    public bool isCaught = false;
    public int tokens = 0;
}

[Serializable]
public class Encounter
{
    public static int MAX_CATCH_TRIES { get { return 3; } }

    public ElementalId EncounterId { get; private set; }
    public int Tries { get; private set; }
    public EncounterState state { get; private set; }

    public Encounter()
    {
        EncounterId = ElementalId.None;
        Tries = 0;
    }

    [JsonConstructor]
    public Encounter(ElementalId EncounterId, int Tries, EncounterState state)
    {
        this.EncounterId = EncounterId;
        this.Tries = Tries;
        this.state = state;
    }

    public void SetNewEncounter(ElementalId encounterId)
    {
        EncounterId = encounterId;
        Tries = 0;
        state = EncounterState.InProgress;
    }

    public void UseTry(bool isCaught)
    {
        Tries++;

        if (isCaught)
        {
            state = EncounterState.Caught;
            return;
        }

        if (Tries >= MAX_CATCH_TRIES)
        {
            state = EncounterState.OutOfTries;
        }
    }

    public bool HasRemainingTries()
    {
        return state != EncounterState.Caught;
    }
}

[Serializable]
public class ElementalManagerState
{
    public List<ElementalEntry> entries { get; private set; }
    public Encounter lastEncounter { get; private set; }
    public Dictionary<ElementalId, List<SkillId>> equipedSkills { get; private set; }

    public ElementalManagerState()
    {
        entries = new List<ElementalEntry>();
        lastEncounter = new Encounter();
        equipedSkills = new Dictionary<ElementalId, List<SkillId>>();
    }

    [JsonConstructor]
    public ElementalManagerState(List<ElementalEntry> entries, Encounter lastEncounter, Dictionary<ElementalId, List<SkillId>> equipedSkills)
    {
        this.entries = entries;
        this.lastEncounter = lastEncounter;
        this.equipedSkills = equipedSkills;
    }
}