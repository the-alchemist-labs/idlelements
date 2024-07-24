
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementalEncounterPanel : MonoBehaviour
{
    [SerializeField] Image encounterImage;
    [SerializeField] TMP_Text nameText;

    public void UpdateUI(Elemental elemental)
    {
        encounterImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        nameText.text = elemental.name;
    }
}
