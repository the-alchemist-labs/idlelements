using System;
using System.Collections.Generic;
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
        InventoryCatalogData inventory = DataService.Instance.LoadData<InventoryCatalogData>(FileName.InventoryCatalog, false);
        Balls = inventory.Balls;
    }

    public Ball GetBall(BallId id)
    {
        return Balls.Find(b => b.Id == id);
    }
}