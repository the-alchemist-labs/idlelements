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
    private int counter = 0;
    void Start()
    {
        currentMap = State.Maps.GetMap(State.currentMap);
        mapNameText.text = currentMap.name;
        Enumerable.Range(0, currentMap.mapElementalTypes.Length)
        .ToList()
        .ForEach(elementalType => Instantiate(typePrefabs[elementalType], typeContainer));
    }

    void Update()
    {
        counter = State.Maps.GetMapProgression(State.currentMap).catchProgression;
        mapProgressionText.text = $"Map Progression: {GetProgressionString()}";
        mapProgressionSlider.value = GetProgressionPercent();
    }

    private float GetProgressionPercent()
    {
        if (counter == 500) return 1;
        if (counter == 0) return 0;
        return counter / 500f;
    }

    private string GetProgressionString(){
        int catchesToComplete = State.Maps.GetMap(State.currentMap).catchesToComplete;
        return  State.Maps.IsMapCompleted(State.currentMap) ? "Clear" : $"{counter}/{TextUtil.NumberFormatter(catchesToComplete)}";
    }
}
