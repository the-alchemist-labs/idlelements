using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapOption : MonoBehaviour
{
    public MapId mapId;
    public TMP_Text mapNameText;
    public Image mapIcon;
    public Material blackAndWhiteMaterial;
    public AudioSource selectMapSound;

    private Material originalMaterial;
    private GameObject allMapsPanel;

    void Start()
    {
        allMapsPanel = GameObject.FindGameObjectWithTag(Tags.MapsPanel);
        GameEvents.OnLevelUp += UpdateUI;
        UpdateUI();
    }

    void OnDestroy()
    {
        GameEvents.OnLevelUp -= UpdateUI;

    }

    public void ChooseMap()
    {   
        if (Player.Instance.Level >= MapCatalog.Instance.GetMap(mapId).requiredLevel)
        {
            MapManager.Instance.UpdateCurrentMap(mapId);
            SoundManager.Instance.PlaySFXFromPrefab(selectMapSound);
            allMapsPanel.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if (Player.Instance.Level >= MapCatalog.Instance.GetMap(mapId).requiredLevel)
        {
            mapNameText.text = MapCatalog.Instance.GetMap(mapId).name;
            mapIcon.material = originalMaterial;
        }
        else
        {
            mapNameText.text = $"Unlock level {MapCatalog.Instance.GetMap(mapId).requiredLevel}";
            mapIcon.material = blackAndWhiteMaterial;
        }
    }
}
