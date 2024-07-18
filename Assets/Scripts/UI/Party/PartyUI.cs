using UnityEngine;
using UnityEngine.UI;

public class PartyUI : MonoBehaviour
{
    public Image[] partyImages;
    public GameObject selectMemberPanel;
    public SelectPartyScrollView selectPartyScript;

    void Start()
    {
        GameEvents.OnPartyUpdated += UpdateUI;
        UpdateUI();
    }


    void OnDestroy()
    {
        GameEvents.OnPartyUpdated -= UpdateUI;
    }

    void UpdateUI()
    {
        for (int i = 0; i < State.party.Length; i++)
        {
            Sprite sprite = (State.party.GetPartyMember(i) != null)
                ? Resources.Load<Sprite>($"Sprites/Elementals/{State.party.GetPartyMember(i)}")
                : Resources.Load<Sprite>($"Sprites/UI/add");

            partyImages[i].sprite = sprite;
        }
    }

    public void OpenSelectMemberPanel(int slot)
    {
        selectMemberPanel.SetActive(true);
        selectPartyScript.Setup(slot);
    }
}