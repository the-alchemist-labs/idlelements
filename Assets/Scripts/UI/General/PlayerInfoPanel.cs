using System.Collections.Generic;
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
        deckText.text = $"Deck: {player.elementalsCaught}/{ElementalCatalog.Instance.Count}";
        battleTowerText.text = "Battle tower: Unavailable";

        partyContainer.Cast<Transform>().ToList().ForEach(child => Destroy(child.gameObject));

        ElementalId[] party = new ElementalId[] {
            Player.Instance.Party.First,
            Player.Instance.Party.Second,
            Player.Instance.Party.Third
        };

        foreach (ElementalId id in party)
        {
            if (id == ElementalId.None) continue;

            GameObject member = Instantiate(partyMemberPrefub, partyContainer);
            if (member.TryGetComponent(out PartyMemberImagePrefab item))
            {
                item.Init(id);
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
