using System.Linq;
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
        mapNameText.text = MapManager.Instance.currentMap.name;

        encountersContainer.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (ElementalEncounter encounter in MapManager.Instance.currentMap.elementalEncounters)
        {
            GameObject newEntry = Instantiate(encounterPrefub, encountersContainer);
            if (newEntry.TryGetComponent(out EncounterPrefub item))
            {   
                Elemental elemental = ElementalCatalog.Instance.GetElemental(encounter.elementalId);
                ElementalEntry entry = ElementalManager.Instance.GetElementalEntry(encounter.elementalId);
                item.UpdateIU(encounter.elementalId, elemental.type,  elemental.name, encounter.encounterChance, entry.tokens);
            }
        }
    }
}