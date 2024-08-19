using TMPro;
using UnityEngine;

public class FriendCode : MonoBehaviour
{
    public TMP_Text friendCodeText;
    public TMP_InputField friendCodeInput;

    void Start()
    {
         friendCodeText.text = $"#{Player.Instance.FriendCode}";
    }

    public async void SendFriendRequest()
    {
        StatusResponse res = await Player.Instance.Friends.SendFriendRequest(friendCodeInput.text);
        RequestResultPopup popup = PopupManager.Instance.OpenPopUp<RequestResultPopup>(PopupId.FriendRequest);
        popup.Init(res);
    }
}
