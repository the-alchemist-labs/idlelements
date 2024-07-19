using TMPro;
using UnityEngine;

public class MapName : MonoBehaviour
{
    public TMP_Text mapNameText;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateMapName;
        UpdateMapName();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateMapName;
    }

    public void UpdateMapName()
    {
        mapNameText.text = MapsData.Instance.currentMap.name;
    }
}