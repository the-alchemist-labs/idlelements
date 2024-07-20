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
        GameEvents.OnPlayerInitialized += UpdateUI;
    }


    void OnDestroy()
    {
        GameEvents.OnPartyUpdated -= UpdateUI;
        GameEvents.OnPlayerInitialized -= UpdateUI;

    }

    void UpdateUI()
    {
        for (int i = 0; i < Player.Instance.Party.MaxSize; i++)
        {
            Sprite sprite = (Player.Instance.Party.GetPartyMember(i) != ElementalId.None)
                ? Resources.Load<Sprite>($"Sprites/Elementals/{Player.Instance.Party.GetPartyMember(i)}")
                : Resources.Load<Sprite>($"Sprites/UI/add");

            partyImages[i].sprite = sprite;
        }
    }

    public void OpenSelectMemberPanel(int slot)
    {
        selectMemberPanel.SetActive(true);
        selectPartyScript.Init(slot);
    }
}