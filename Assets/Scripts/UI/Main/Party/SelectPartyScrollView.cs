using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartyScrollView : MonoBehaviour
{
    public GameObject panel;
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject prefub;
    public AudioSource selectSound;

    private ElementalId? selectedElemental;
    private int memberSlot;

    public void Setup(int slot)
    {
        memberSlot = slot;
        selectedElemental = State.party.GetPartyMember(slot);

        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        UpdateUI();


        // Scroll to top of the scrollview
        scrollRect.verticalNormalizedPosition = 1f;
    }

    void UpdateUI()
    {
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (ElementalId entryId in State.Elementals.GetEligiblePartyMembers())
        {
            GameObject newEntry = Instantiate(prefub, scrollViewContent);
            if (newEntry.TryGetComponent(out AvailablePartyMemberPrefub item))
            {
                item.SetPartyMemberOption(memberSlot, entryId, selectedElemental == entryId, OnMemberChanged);
            }
        }
    }
    void OnMemberChanged(ElementalId selectedId)
    {
        selectedElemental = selectedId;
        UpdateUI();
    }

    public void OnMemberSelected()
    {
        State.party.SetPartyMember(memberSlot, selectedElemental);
        selectSound.Play();
        panel.SetActive(false);
    }
}
