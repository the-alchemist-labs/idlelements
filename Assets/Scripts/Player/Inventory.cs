using System.Collections.Generic;
using System.Linq;

public enum BallId
{
    Normal,
    Super,
    Master
}

public class Ball
{
    public BallId Id;
    public string Name;
    public string Description;
    public float CatchRate;
    public int Amount;

    public Ball(){}
}

public class Inventory
{
    public List<Ball> Balls { get; private set; }

    public Inventory()
    {
        Balls = new List<Ball>();
    }
    public Inventory(List<Ball> Balls = null)
    {
        this.Balls = Balls ?? new List<Ball>();
    }

    public void UseBall(BallId ballId, int amount)
    {
        Balls.Where(b => b.Id == ballId).ToList().ForEach(b => b.Amount -= amount);
    }

    public void AddBall(Ball ball)
    {
        if (Balls.Any(b => b.Id == ball.Id))
        {
            Balls.Where(b => b.Id == ball.Id).ToList().ForEach(b => b.Amount += ball.Amount);
        }
        else
        {
            Balls.Add(ball);
        }
    }
}