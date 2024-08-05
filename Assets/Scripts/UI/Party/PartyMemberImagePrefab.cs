using UnityEngine;
using UnityEngine.UI;

public class PartyMemberImagePrefab : MonoBehaviour
{
    public Image typeColor;
    public Image elementalImage;

    public void Init(ElementalId elementalId)
    {
        if (elementalId == ElementalId.None)
        {
            typeColor.sprite = Resources.Load<Sprite>("Sprites/UI/add");
            elementalImage.sprite = null;
            return;
        }

        Elemental elemental = ElementalCatalog.Instance.GetElemental(elementalId);

        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.Id}");
        typeColor.color = Types.GetElementalColor(elemental.Type);
    }
}
