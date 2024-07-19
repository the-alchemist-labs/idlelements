using UnityEngine;
using UnityEngine.UI;

public class PartyMemberPrefab : MonoBehaviour
{
    public Image typeColor;
    public Image elementalImage;

    public void Init(ElementalId elementalId)
    {
        Elemental elemental = ElementalCatalog.Instance.GetElemental(elementalId);

        Sprite newSprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.id}");
        elementalImage.sprite = newSprite;
        typeColor.color = Types.GetElementalColor(elemental.type);
    }
}
