using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<BallId, int> Balls { get; private set; }
    public Dictionary<ElementType, int> Elementokens { get; private set; }

    public Inventory()
    {
        Elementokens = new Dictionary<ElementType, int>();
        Balls = new Dictionary<BallId, int> {
            { BallId.Normal, 5},
        };
    }
    
    public Inventory(Dictionary<BallId, int> Balls = null, Dictionary<ElementType, int> Elementokens = null)
    {
        this.Balls = Balls ?? new Dictionary<BallId, int>();
        this.Elementokens = Elementokens ?? new Dictionary<ElementType, int>();
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
        if (amount > 0)
        {
            NotificationManager.Instance.PostNotification($"Gained {amount} {ballId}{(amount > 1 ? "s" : "")}.");
        }
    }

    public void UpdateTokens(ElementType tokenType, int amount)
    {
        if (!Elementokens.ContainsKey(tokenType))
        {
            Elementokens[tokenType] = 0;
        }

        if (Elementokens[tokenType] + amount < 0)
        {
            Debug.LogError($"Player don't have enought {tokenType} Elementokens");
            return;
        }

        Elementokens[tokenType] += amount;
        GameEvents.ElementokensUpdated();
        if (amount > 0)
        {
            NotificationManager.Instance.PostNotification($"Gained {amount} {tokenType} Elementoken {(amount > 1 ? "s" : "")}.");
        }
    }
}