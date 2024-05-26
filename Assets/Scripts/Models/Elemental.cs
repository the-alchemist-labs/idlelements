using System;

[Serializable]
public class Elemental
{
    public int idNum;
    public string name;
    public ElementType type;
    public float catchRate;

    public Elemental(int idNum, string name, ElementType type, float catchRate)
    {
        this.idNum = idNum;
        this.name = name;
        this.type = type;
        this.catchRate = catchRate;
    }

}