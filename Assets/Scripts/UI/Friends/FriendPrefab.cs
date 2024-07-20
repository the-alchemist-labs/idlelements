using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendPrefab : MonoBehaviour
{
    public TMP_Text playreNameText;
    public TMP_Text playerLevelText;
    public Image onlineIndicator;

    private PlayerInfo playerInfo;

    public void Init(PlayerInfo fromPlayer)
    {
        playerInfo = fromPlayer;
        onlineIndicator.color = fromPlayer.isOnline ? new Color(0, 1, 0) : new Color(90f / 255f, 100f / 255f, 125f / 255f);
        playreNameText.text = fromPlayer.name;
        playerLevelText.text = $"{fromPlayer.level}";
    }

    public void OpenFriendPanel()
    {
        MainManager gameManger = GameObject.FindGameObjectWithTag(Tags.MainManager).GetComponent<MainManager>();
        gameManger.playerInfoPanel?.SetActive(true);
        gameManger.playerInfoPanel?.GetComponent<PlayerInfoPanel>().Init(playerInfo);
    }
}
