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
        if (Player.Instance.Level >= MapsData.Instance.GetMap(mapId).requiredLevel)
        {
            MapsData.Instance.UpdateCurrentMap(mapId);
            selectMapSound.Play();
            allMapsPanel.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if (Player.Instance.Level >= MapsData.Instance.GetMap(mapId).requiredLevel)
        {
            mapNameText.text = MapsData.Instance.GetMap(mapId).name;
            mapIcon.material = originalMaterial;
        }
        else
        {
            mapNameText.text = $"Unlock level {MapsData.Instance.GetMap(mapId).requiredLevel}";
            mapIcon.material = blackAndWhiteMaterial;
        }
    }
}
