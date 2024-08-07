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
            elementalImage.sprite = Resources.Load<Sprite>("Sprites/UI/add");;
            return;
        }

        Elemental elemental = ElementalCatalog.Instance.GetElemental(elementalId);

        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{elemental.Id}");
        typeColor.color = Types.GetElementalColor(elemental.Type);
    }
}
