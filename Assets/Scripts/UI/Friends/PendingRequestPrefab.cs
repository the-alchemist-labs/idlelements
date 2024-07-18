using TMPro;
using UnityEngine;

public class PendingRequestPrefab : MonoBehaviour
{
    public TMP_Text playreNameText;

    private PlayerInfo requestingPlayer;

    public void Init(PlayerInfo fromPlayer)
    {
        requestingPlayer = fromPlayer;
        playreNameText.text = fromPlayer.name;
    }

    public async void AcceptRequest()
    {
        Status res = await Player.Instance.FriendRequestRespond(requestingPlayer.id, Respond.Accept);
        // display status
    }

    public async void RejectRequest()
    {
        Status res = await Player.Instance.FriendRequestRespond(requestingPlayer.id, Respond.Reject);
    }
}
