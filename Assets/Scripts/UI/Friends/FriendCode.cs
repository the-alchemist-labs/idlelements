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
        RequestResultPanel popup = PopupManager.Instance.OpenPopUp<RequestResultPanel>(PopupId.FriendRequest);
        popup.Init(res);
    }
}
