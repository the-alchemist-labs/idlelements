using System.Collections.Generic;
using UnityEngine;

public enum BallId
{
    None,
    Normal,
    Great,
    Master
}

public class Ball
{
    public BallId Id;
    public string Name;
    public string Description;
    public float CatchRate;

    public Ball() { }
}

public class Inventory
{
    public Dictionary<BallId, int> Balls { get; private set; }

    public Inventory()
    {
        Balls = new Dictionary<BallId, int> {
            { BallId.Normal, 0},
        };
    }

    public Inventory(Dictionary<BallId, int> Balls = null)
    {
        this.Balls = Balls ?? new Dictionary<BallId, int>();
    }

    public void UpdateBalls(BallId ballId, int amount)
    {
        if (!Balls.ContainsKey(ballId))
        {
            Balls[ballId] = 0;
        }

        if (Balls[ballId] + amount < 0)
        {
            Debug.LogError($"Player don't have enought {ballId} balls");
            return;
        }

        Balls[ballId] += amount;
        GameEvents.BallsUpdated();
    }
}