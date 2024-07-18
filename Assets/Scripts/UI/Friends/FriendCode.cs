using TMPro;
using UnityEngine;

public class FriendCode : MonoBehaviour
{
    public TMP_Text friendCodeText;
    public TMP_InputField friendCodeInput;
    public GameObject requestResultPanel;

    private string inputValue;
    void Start()
    {
        GameEvents.OnFriendsUpdated += SetFriendCode;
        friendCodeInput?.onValueChanged.AddListener(OnInputFriendCodeChanged);
    }

    void OnDestroy()
    {
        GameEvents.OnFriendsUpdated -= SetFriendCode;
    }

    public async void SendFriendRequest()
    {
        StatusResponse res = await Player.Instance.Friends.SendFriendRequest(friendCodeInput.text);
        requestResultPanel.SetActive(true);
        requestResultPanel.GetComponent<RequestResultPanel>().Init(res);
    }

    private void OnInputFriendCodeChanged(string input)
    {
        if (input == inputValue) return;

        inputValue = input.ToUpper();
        friendCodeInput.text = inputValue;
    }

    private void SetFriendCode()
    {
        friendCodeText.text = $"#{Player.Instance.FriendCode}";

    }
}
