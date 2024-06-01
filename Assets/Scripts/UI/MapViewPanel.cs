using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapViewPanel : MonoBehaviour
{
    public TMP_Text mapNameText;
    public TMP_Text mapProgressionText;
    public Slider mapProgressionSlider;
    public Transform typeContainer;
    public GameObject[] typePrefabs;

    private Map currentMap;
    private int counter = 10;
    void Start()
    {
        currentMap = Maps.GetMap(State.currentMap);
        mapNameText.text = currentMap.name;
        Enumerable.Range(0, currentMap.mapElementalTypes.Length)
        .ToList()
        .ForEach(elementalType => Instantiate(typePrefabs[elementalType], typeContainer));
    }

    void Update()
    {
        if (counter < 500) counter++;

        mapProgressionText.text = $"Map Progression: {counter}/500";
        mapProgressionSlider.value = GetProgressionPercent();
    }

    private float GetProgressionPercent()
    {
        if (counter == 500) return 1;
        if (counter == 0) return 0;
        return (float)counter / 500f;
    }
}
