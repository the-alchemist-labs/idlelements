using TMPro;
using UnityEngine;

public class FriendPrefab : MonoBehaviour
{
    public TMP_Text playreNameText;

    public void Init(PlayerInfo fromPlayer)
    {
        playreNameText.text = fromPlayer.name;
    }
}
