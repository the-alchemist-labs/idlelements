using TMPro;
using UnityEngine;

public class FriendCode : MonoBehaviour
{
    public TMP_Text friendCodeText;
    public TMP_InputField friendCodeInput;

    private string inputValue;
    void Start()
    {
        GameEvents.OnPlayerInitilized += SetFriendCode;
        friendCodeInput.onValueChanged.AddListener(OnInputFriendCodeChanged);

    }

    void OnDestroy()
    {
        GameEvents.OnPlayerInitilized -= SetFriendCode;
    }

    public async void SendFriendRequest()
    {
        Status res = await Player.Instance.SendFriendRequest(friendCodeInput.text.ToUpper());
        Debug.Log(res);
    }

    private void OnInputFriendCodeChanged(string input)
    {
        if (input == inputValue) return;

        inputValue = input.ToUpper().Replace("#", "");
        friendCodeInput.text = $"#{inputValue}";
    }

    private void SetFriendCode()
    {
        friendCodeText.text = $"#{Player.Instance.FriendCode}";

    }
}
