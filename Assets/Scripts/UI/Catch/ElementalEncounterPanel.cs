
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementalEncounterPanel : MonoBehaviour
{
    [SerializeField] Image encounterImage;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text triesText;
    [SerializeField] TMP_Text encounterStateText;

    public void UpdateUI(Encounter encounter)
    {
        Elemental elemental = ElementalCatalog.Instance.GetElemental(encounter.EncounterId);
        encounterImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        nameText.text = elemental.name;
        triesText.text = $"Tries left: {Encounter.MAX_CATCH_TRIES - encounter.Tries}";
        encounterStateText.text = $"State: {encounter.state}";
    }
}
