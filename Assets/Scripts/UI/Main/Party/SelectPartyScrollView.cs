using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartyScrollView : MonoBehaviour
{
    public GameObject panel;
    public ScrollRect scrollRect;
    public Transform scrollViewContent;
    public GameObject prefub;
    public TMP_Text idleBonusText;
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
        idleBonusText.text = $"Idle bonus: {GetIdleBonus()}";
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

    string GetIdleBonus()
    {
        if (selectedElemental == null)
        {
            return "None";
        }
        else
        {
            Elemental elemental = State.Elementals.GetElemental((ElementalId)selectedElemental);
            return elemental.idleBonus != null ? $"{elemental.idleBonus.amount * 100}% {elemental.idleBonus.resource}" : "None";
        }
    }
}
