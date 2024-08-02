using UnityEngine;

public enum BallId
{
    None,
    Normal,
    Great,
    Master
}

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory/Balls")]
public class Ball: ScriptableObject
{
    public BallId Id;
    public string Name;
    public string Description;
    public float CatchRate;
}