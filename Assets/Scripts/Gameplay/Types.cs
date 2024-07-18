using UnityEngine;

public enum ElementType
{
    Fire,
    Water,
    Earth,
    Air,
    Ice,
    Lightning,
    Chaos
}

public static class Types
{
    public static Color GetElementalColor(ElementType type)
    {
        switch (type)
        {
            case ElementType.Fire:
                return new Color(101f / 255f, 0f, 0f);
            case ElementType.Water:
                return new Color(14 / 255f, 71f / 255f, 120f / 255f);
            case ElementType.Earth:
                return new Color(21 / 255f, 67f / 255f, 24f / 255f);
            case ElementType.Air:
                return Color.white;
            case ElementType.Ice:
                return Color.cyan;
            case ElementType.Lightning:
                return Color.yellow;
            case ElementType.Chaos:
                return Color.magenta;
            default:
                return Color.white;
        }
    }
}
