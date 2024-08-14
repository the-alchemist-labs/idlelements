using System;

public enum RewardId
{
    Gold,
    Essence,
    Orbs,
    Exp,
    NormalBall,
    GreatBall,
    MasterBall,
    FireToken,
    WaterToken,
    AirToken,
    EarthToken,
    LightningToken,
    IceToken,
    ChaosToken,
}

public enum RewardType
{
    Gold,
    Essence,
    Orbs,
    Exp,
    Ball,
    Token
}

[Serializable]
public class Reward
{
    public RewardId Id;
    public RewardType Type;
    public int Amount;
    public float Chance;
}
