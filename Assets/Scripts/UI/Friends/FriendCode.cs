using TMPro;
using UnityEngine;

public class FriendCode : MonoBehaviour
{
    public TMP_Text friendCodeText;
    public TMP_InputField friendCodeInput;
    public GameObject requestResultPanel;

    void Start()
    {
         friendCodeText.text = $"#{Player.Instance.FriendCode}";
    }

    public async void SendFriendRequest()
    {
        StatusResponse res = await Player.Instance.Friends.SendFriendRequest(friendCodeInput.text);
        requestResultPanel.SetActive(true);
        requestResultPanel.GetComponent<RequestResultPanel>().Init(res);
    }
}
