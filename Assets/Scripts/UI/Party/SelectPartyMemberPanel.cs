using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartyMemberPanel : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform scrollViewContent;
    [SerializeField] GameObject prefub;
    [SerializeField] SelectMemberInfo selectMemberInfo;

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
            GameObject newEntry = Instantiate(prefub, scrollViewContent);
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
