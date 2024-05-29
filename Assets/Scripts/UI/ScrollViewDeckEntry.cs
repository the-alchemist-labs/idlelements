using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scrollViewDeckEntry : MonoBehaviour
{
    public Image elementalImage;
    public TMP_Text ElementalId;
    public TMP_Text ElementalName;

    public void UpdateEntry(Elemental elemental)
    {
        ElementalId.text = $"#{(int)elemental.id}";
        ElementalName.text = elemental.name;
    }
}