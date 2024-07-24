using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BallId
{
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
        Balls = new Dictionary<BallId, int>();
    }
    
    public Inventory(Dictionary<BallId, int> Balls = null)
    {
        this.Balls = Balls ?? new Dictionary<BallId, int>();
    }

    public void UpdateBalls(BallId ballId, int amount)
    {
        if (!Balls.ContainsKey(ballId) && amount < 0)
        {
            Debug.LogError($"Can't subtract ${amount} balls from {ballId} that the player doesn't have");
            return;
        }

        if (!Balls.ContainsKey(ballId))
        {
            Balls[ballId] = 0;
        }

        Balls[ballId] += amount;
        GameEvents.BallsUpdated();
    }
}