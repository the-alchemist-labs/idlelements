using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartyMemberPopup : BasePopup
{
    public override PopupId Id { get; } = PopupId.SelectPartyMember;
    
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject memberPrefab;
    [SerializeField] private SelectMemberInfo selectMemberInfo;

    private ElementalId _selectedElemental;
    private int _memberSlot;

    public void OpenPanel(int slot)
    {
        gameObject.SetActive(true);

        _memberSlot = slot;
        _selectedElemental = Player.Instance.Party.GetPartyMember(slot);

        UpdateScrollView();
        selectMemberInfo.Init(_selectedElemental);
    }

    void UpdateScrollView()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (ElementalId entryId in Player.Instance.Party.GetEligiblePartyMembers())
        {
            GameObject newEntry = Instantiate(memberPrefab, scrollViewContent);
            if (newEntry.TryGetComponent(out AvailablePartyMemberPrefub item))
            {
                item.SetPartyMemberOption(_memberSlot, entryId, _selectedElemental == entryId);
                item.OnMemberSelected += MemberChanged;
            }
        }
    }

    public void OnSelectClicked()
    {
        Player.Instance.Party.SetPartyMember(_memberSlot, _selectedElemental);
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);

        gameObject.SetActive(false);
    }

    private void MemberChanged(ElementalId selectedId)
    {
        _selectedElemental = selectedId;

        UpdateScrollView();
        selectMemberInfo.Init(selectedId);
    }
}
