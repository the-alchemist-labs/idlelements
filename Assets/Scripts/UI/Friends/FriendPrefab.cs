using TMPro;
using UnityEngine;

public class FriendPrefab : MonoBehaviour
{
    public TMP_Text playreNameText;
    public TMP_Text playerLevelText;

    private PlayerInfo playerInfo;

    public void Init(PlayerInfo fromPlayer)
    {
        playerInfo = fromPlayer;
        playreNameText.text = fromPlayer.name;
        playerLevelText.text = $"{fromPlayer.level}";
    }

    public void OpenFriendPanel()
    {
        GameManager gameManger = GameObject.FindGameObjectWithTag(Tags.GameManager).GetComponent<GameManager>();
        gameManger.playerInfoPanel?.SetActive(true);
        gameManger.playerInfoPanel?.GetComponent<PlayerInfoPanel>().Init(playerInfo);
    }
}
