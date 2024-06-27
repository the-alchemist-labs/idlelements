using TMPro;
using UnityEngine;

public class MapInfoPanel : MonoBehaviour
{
    public TMP_Text mapNameText;
    public Transform encountersContainer;
    public GameObject encounterPrefub;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateUI;
    }

    public void UpdateUI()
    {
        mapNameText.text = State.Maps.currentMap.name;

        foreach (ElementalEncounter encounter in State.Maps.currentMap.elementalEncounters)
        {
            GameObject newEntry = Instantiate(encounterPrefub, encountersContainer);
            if (newEntry.TryGetComponent(out EncounterPrefub item))
            {   
                Elemental elemental = State.Elementals.GetElement(encounter.elementalId);
                ElementalEntry entry = State.Elementals.GetElementalEntry(encounter.elementalId);
                item.UpdateIU(encounter.elementalId, elemental.type,  elemental.name, encounter.encounterChance, entry.tokens);
            }
        }
    }
}