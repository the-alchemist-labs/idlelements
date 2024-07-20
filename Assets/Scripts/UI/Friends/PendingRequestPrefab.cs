using TMPro;
using UnityEngine;

public class PendingRequestPrefab : MonoBehaviour
{
    public TMP_Text playreNameText;
    public AudioSource respondSound;

    private PlayerInfo requestingPlayer;

    public void Init(PlayerInfo fromPlayer)
    {
        requestingPlayer = fromPlayer;
        playreNameText.text = fromPlayer.name;
    }

    public async void AcceptRequest()
    {
        await Player.Instance.Friends.FriendRequestRespond(requestingPlayer.id, Respond.Accept);
        SoundManager.Instance.PlaySFXFromPrefab(respondSound);

    }

    public async void RejectRequest()
    {
        await Player.Instance.Friends.FriendRequestRespond(requestingPlayer.id, Respond.Reject);
        SoundManager.Instance.PlaySFXFromPrefab(respondSound);

    }
}
