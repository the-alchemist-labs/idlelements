using UnityEngine;
using UnityEngine.UI;

public class ChangeImageToElemental : MonoBehaviour
{
    public Image image;

    public void UpdateImageToElemental(ElementalId id)
    {   
        Sprite newSprite = Resources.Load<Sprite>($"Sprites/Elementals/{id}");
        image.sprite = newSprite;
    }
}
