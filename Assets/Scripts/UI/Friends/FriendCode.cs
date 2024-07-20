using TMPro;
using UnityEngine;

public class FriendCode : MonoBehaviour
{
    public TMP_Text friendCodeText;
    public TMP_InputField friendCodeInput;
    public GameObject requestResultPanel;

    void Start()
    {
        GameEvents.OnPlayerInitialized += SetFriendCode;
         friendCodeText.text = $"#{Player.Instance.FriendCode}";
    }

    void OnDestroy()
    {
        GameEvents.OnPlayerInitialized -= SetFriendCode;
    }

    public async void SendFriendRequest()
    {
        StatusResponse res = await Player.Instance.Friends.SendFriendRequest(friendCodeInput.text);
        requestResultPanel.SetActive(true);
        requestResultPanel.GetComponent<RequestResultPanel>().Init(res);
    }

    private void SetFriendCode()
    {
        friendCodeText.text = $"#{Player.Instance.FriendCode}";
    }
}
