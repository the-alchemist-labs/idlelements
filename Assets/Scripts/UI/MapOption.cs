using TMPro;
using UnityEngine;

public class MapOption : MonoBehaviour
{
    public MapId mapId;
    public TMP_Text mapNameText;
    public GameObject mapsPanel;

    void Start()
    {
        mapNameText.text = mapId.ToString();
    }

    public void ChooseMap()
    {
        State.Maps.UpdateCurrentMap(mapId);
        mapsPanel.SetActive(false);
    }
}
