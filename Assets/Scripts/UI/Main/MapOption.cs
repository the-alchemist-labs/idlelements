using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapOption : MonoBehaviour
{
    public MapId mapId;
    public TMP_Text mapNameText;
    public Image mapIcon;
    public Material blackAndWhiteMaterial;

    private Material originalMaterial;
    private Script panelHandler;

    void Start()
    {
        panelHandler = FindGameObjectWithTag("MapsPanel").GetComponent<DisplayPanel>();
        GameEvents.OnLevelUp += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnLevelUp -= UpdateUI;

    }

    public void ChooseMap()
    {   
        if (State.level >= State.Maps.GetMap(mapId).requiredLevel)
        {
            State.Maps.UpdateCurrentMap(mapId);
            panelHandler.ClosePanel();
        }
    }

    private void UpdateUI()
    {
        if (State.level >= State.Maps.GetMap(mapId).requiredLevel)
        {
            mapNameText.text = State.Maps.GetMap(mapId).name;
            mapIcon.material = originalMaterial;
        }
        else
        {
            mapNameText.text = $"Unlock level {State.Maps.GetMap(mapId).requiredLevel}";
            mapIcon.material = blackAndWhiteMaterial;
        }
    }
}
