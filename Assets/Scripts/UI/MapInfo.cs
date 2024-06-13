using System.Linq;
using TMPro;
using UnityEngine;

public class MapViewPanel : MonoBehaviour
{
    public GameObject allMapsPanel;
    public TMP_Text mapNameText;
    public Transform typeContainer;
    public GameObject[] typePrefabs;
    public TMP_Text encounterTimeText;
    public TMP_Text goldGainText;
    public TMP_Text essanceGainText;


    private Map currentMap;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateDisaplayedMapData;
        UpdateDisaplayedMapData();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateDisaplayedMapData;
    }

    public void UpdateDisaplayedMapData()
    {
        currentMap = State.Maps.currentMap;
        mapNameText.text = currentMap.name;
        encounterTimeText.text = $"Encounter time: {State.GetEncounterSpeed()} seconds";

        float goldMinuteMultiplier = 60 / GoldMine.incomeLoopSeconds;
        goldGainText.text = $"Gold gain: {TextUtil.NumberFormatter((int)(GoldMine.GetTotalGoldFromAllMaps() * goldMinuteMultiplier))} per minute";
        
        float essenceMinuteMultiplier = 60 / EssenceLab.incomeLoopSeconds;
        essanceGainText.text = $"Essence gain: {TextUtil.NumberFormatter((int)(EssenceLab.GetTotalEssenceFromAllMaps() * essenceMinuteMultiplier))} per minute";

        typeContainer.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        Enumerable.Range(0, currentMap.mapElementalTypes.Length)
        .ToList()
        .ForEach(elementalType => Instantiate(typePrefabs[elementalType], typeContainer));
        // need to make generic type prefub and update the color and text inside
    }
}
