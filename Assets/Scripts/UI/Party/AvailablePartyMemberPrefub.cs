using System;
using UnityEngine;
using UnityEngine.UI;

public class AvailablePartyMemberPrefub : MonoBehaviour
{
    public event Action<ElementalId> OnMemberSelected;

    [SerializeField] Button button;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image elementalImage;
    [SerializeField] Image inPartyImage;

    private ElementalId _elementalId;

    public void SetPartyMemberOption(int slot, ElementalId id, bool isSelectedElemental)
    {
        _elementalId = id;
        backgroundImage.color = isSelectedElemental ? Color.white : Color.grey;
        elementalImage.sprite = Resources.Load<Sprite>($"Sprites/Elementals/{id}");

        bool shouldAllowToSelect = ShouldAllowToSelect(slot, id);
        inPartyImage.gameObject.SetActive(!shouldAllowToSelect);
        button.interactable = shouldAllowToSelect;
    }

    public void OnSelected()
    {
        OnMemberSelected.Invoke(_elementalId);
    }

    public bool ShouldAllowToSelect(int slot, ElementalId id)
    {
        return !Player.Instance.Party.IsInParty(id) || Player.Instance.Party.GetPartyMember(slot) == id;
    }
}