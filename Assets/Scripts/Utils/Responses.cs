[System.Serializable]
public class FriendCodeResponse
{
    public string code;
}

[System.Serializable]
public class FriendRequestResponse
{
    public Status status;
}

public enum Status
{
    Success,
    Failed
}