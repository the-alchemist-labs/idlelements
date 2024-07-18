using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    public TMP_Text levelText;
    public TMP_Text playerName;
    public TMP_Text deckText;
    public TMP_Text battleTowerText;
    public Transform partyContainer;
    public GameObject partyMemberPrefub;
    public GameObject friendActionsContainer;
    public GameObject playerActionsContainer;

    private string playerId;

    public void Init(PlayerInfo player)
    {
        playerId = player.id;
        bool isSelf = playerId == Player.Instance.Id;

        levelText.text = $"{player.level}";
        playerName.text = player.name;
        deckText.text = $"Deck: {player.elementalsCaught}/{State.Elementals.all.Count}";
        battleTowerText.text = "Battle tower: Unavailable";

        partyContainer.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        if (player.party != null)
        {
            foreach (ElementalId request in player.party)
            {
                GameObject member = Instantiate(partyMemberPrefub, partyContainer);
                if (member.TryGetComponent(out PartyMemberPrefab item))
                {
                    item.Init(request);
                }
            }
        }

        friendActionsContainer.SetActive(!isSelf);
        playerActionsContainer.SetActive(isSelf);
    }

    public void Unfriend()
    {
        Debug.Log("Unfriend: " + playerId);
    }

    public void Trade()
    {
        Debug.Log("Trade: " + playerId);
    }

    public void JoinMap()
    {
        Debug.Log("JoinMap: " + playerId);
    }
}
