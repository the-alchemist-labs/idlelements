using System.Linq;
using TMPro;
using UnityEngine;

public class MapViewPanel : MonoBehaviour
{
    public GameObject allMapsPanel;
    public TMP_Text mapNameText;
    public Transform typeContainer;
    public GameObject[] typePrefabs;

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
        typeContainer.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        Enumerable.Range(0, currentMap.mapElementalTypes.Length)
        .ToList()
        .ForEach(elementalType => Instantiate(typePrefabs[elementalType], typeContainer));
        // need to make generic type prefub and update the color and text inside
    }
}
