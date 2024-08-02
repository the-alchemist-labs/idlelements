using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryCatalogData
{
    public List<Ball> Balls { get; }

    public InventoryCatalogData(List<Ball> Balls)
    {
        this.Balls = Balls;
    }

    public InventoryCatalogData()
    {
        Balls = new List<Ball>();
    }
}

public class InventoryCatalog : MonoBehaviour
{
    private string SCRIPTABLE_OBJECT_BALLS_PATH = "ScriptableObjects/Inventory/Balls";

    public static InventoryCatalog Instance { get; private set; }
    public List<Ball> Balls { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        Balls = Resources.LoadAll<Ball>(SCRIPTABLE_OBJECT_BALLS_PATH).ToList();
    }

    public Ball GetBall(BallId id)
    {
        return Balls.Find(b => b.Id == id);
    }
}