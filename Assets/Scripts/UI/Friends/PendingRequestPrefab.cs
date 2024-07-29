using TMPro;
using UnityEngine;

public class PendingRequestPrefab : MonoBehaviour
{
    [SerializeField]
    TMP_Text playreNameText;

    private PlayerInfo _requestingPlayer;

    public void Init(PlayerInfo fromPlayer)
    {
        _requestingPlayer = fromPlayer;
        playreNameText.text = fromPlayer.name;
    }

    public async void AcceptRequest()
    {
        await Player.Instance.Friends.FriendRequestRespond(_requestingPlayer.id, Respond.Accept);
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);
    }

    public async void RejectRequest()
    {
        await Player.Instance.Friends.FriendRequestRespond(_requestingPlayer.id, Respond.Reject);
        SoundManager.Instance.PlaySystemSFX(SystemSFXId.Click);

    }
}
