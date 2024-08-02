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

    private ElementalId selectedElemental;
    private int memberSlot;

    public void Init(int slot)
    {
        memberSlot = slot;
        selectedElemental = Player.Instance.Party.GetPartyMember(slot);

        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));
        UpdateUI();

        // Scroll to top of the scrollview
        scrollRect.verticalNormalizedPosition = 1f;
    }

    void UpdateUI()
    {
        idleBonusText.text = $"Idle bonus: {GetIdleBonus()}";
        scrollViewContent.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        foreach (ElementalId entryId in Player.Instance.Party.GetEligiblePartyMembers())
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
        Player.Instance.Party.SetPartyMember(memberSlot, selectedElemental);
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Celebration);
        panel.SetActive(false);
    }

    string GetIdleBonus()
    {
        if (selectedElemental == ElementalId.None)
        {
            return "None";
        }
        else
        {
            Elemental elemental = ElementalCatalog.Instance.GetElemental((ElementalId)selectedElemental);
            return elemental.IdleBonus != null ? $"{elemental.IdleBonus.amount * 100}% {elemental.IdleBonus.resource}" : "None";
        }
    }
}
