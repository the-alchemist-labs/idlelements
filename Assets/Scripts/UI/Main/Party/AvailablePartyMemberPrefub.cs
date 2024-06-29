using UnityEngine;
using UnityEngine.UI;

public class AvailablePartyMemberPrefub : MonoBehaviour
{
    public Button button;
    public Image backgroundImage;
    public Image elementalImage;
    public Image inPartyImage;    

    private ElementalId elementalId;
    private System.Action<ElementalId> onMemberChanged;

    public void SetPartyMemberOption(int slot, ElementalId id, bool isSelectedElemental, System.Action<ElementalId> onMemberChanged)
    {
        this.onMemberChanged = onMemberChanged;
        elementalId = id;

        backgroundImage.color = isSelectedElemental ? Color.white : Color.grey;
        string spritePath = $"Sprites/Elementals/{id}";
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        elementalImage.sprite = sprite;

        bool shouldAllowToSelect = ShouldAllowToSelect(slot, id);
        inPartyImage.gameObject.SetActive(!shouldAllowToSelect);
        button.interactable = shouldAllowToSelect;
    }

    public void OnSelected()
    {
        onMemberChanged(elementalId);
    }

    public bool ShouldAllowToSelect(int slot, ElementalId id)
    {
        return !State.party.IsInParty(id) || State.party.GetPartyMember(slot) == id;
    }
}