using System;

public enum ElementalStat
{
    Hp,
    Attack,
    Defense,
    Speed,
}

[Serializable]
public class ElementalStats
{
    public int Hp;
    public int Attack;
    public int Defense;
    public int Speed;

    // public void ModifyStat(ElementalStat stat, int modifyBy)
    // {
    //     switch (stat)
    //     {
    //         case ElementalStat.Attack:
    //             Attack += modifyBy;
    //             break;
    //         case ElementalStat.Defense:
    //             Defense += modifyBy;
    //             break;
    //         case ElementalStat.Speed:
    //             Speed += modifyBy;
    //             break;
    //     }
    // }
}
