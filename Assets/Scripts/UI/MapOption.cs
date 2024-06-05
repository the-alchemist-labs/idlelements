using TMPro;
using UnityEngine;

public class MapOption : MonoBehaviour
{
    public MapId mapId;
    public TMP_Text mapNameText;
    private GameObject mapsPanel;
    private GameObject mapDataComponent;

    void Start()
    {
        mapsPanel = GameObject.FindWithTag(Tags.MapsPanel);
        mapDataComponent = GameObject.FindWithTag(Tags.MapData);
        mapNameText.text = mapId.ToString();
    }

    public void ChooseMap()
    {
        State.UpdateCurrentMap(mapId);
        mapDataComponent?.GetComponent<MapViewPanel>()?.UpdateDisaplayedMapData();
        mapsPanel.SetActive(false);
    }
}
