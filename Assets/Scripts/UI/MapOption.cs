using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapOption : MonoBehaviour
{
    public MapId mapId;
    public TMP_Text mapNameText;
    public Image mapIcon;
    public GameObject mapsPanel;
    public Material blackAndWhiteMaterial;

    private Material originalMaterial;

    void Start()
    {
        GameEvents.OnLevelUp += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnLevelUp -= UpdateUI;

    }

    public void ChooseMap()
    {
        State.Maps.UpdateCurrentMap(mapId);
        mapsPanel.SetActive(false);
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
