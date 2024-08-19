using TMPro;
using UnityEngine;

public class RequestResultPopup : BasePopup
{
    public override PopupId Id { get; } = PopupId.FriendRequest;

    public TMP_Text title;

    private const string FriendRequestSent = "Friend request sent";
    private const string PlayerNotFound = "Player not found";
    private const string RequestAlreadySent = "You sent a request already";
    private const string AlreadyFriends = "You are already friends";
    private const string SomethingWentWrong = "Oops something went wrong";
    
    public void Init(StatusResponse res)
    {
        print(res);

        switch (res.status)
        {
            case Status.Success:
                title.text = FriendRequestSent;
                break;
            default:
                HandleErrorMessage(res.message);
                break;
        }
    }

    private void HandleErrorMessage(string message)
    {
        switch (message)
        {
            case PlayerNotFound:
                title.text = PlayerNotFound;
                break;
            case RequestAlreadySent:
                title.text = RequestAlreadySent;
                break;
            case AlreadyFriends:
                title.text = AlreadyFriends;
                break;
            default:
                title.text = SomethingWentWrong;
                break;
        }
    }
}
