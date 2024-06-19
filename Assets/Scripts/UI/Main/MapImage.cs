using UnityEngine;
using UnityEngine.UI;

public class MapImage : MonoBehaviour
{
    public Image mapImage;

    void Start()
    {
        GameEvents.OnMapDataChanged += UpdateMapImage;
        UpdateMapImage();
    }

    void OnDestroy()
    {
        GameEvents.OnMapDataChanged -= UpdateMapImage;
    }

    public void UpdateMapImage()
    {
        Sprite newSprite = Resources.Load<Sprite>($"Sprites/Maps/{State.Maps.currentMapId}");
        mapImage.sprite = newSprite;
    }
}