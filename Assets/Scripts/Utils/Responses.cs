using System;
using System.Collections.Generic;

[Serializable]
public class FriendCodeResponse
{
    public string code;
}

[Serializable]
public class PendingFriendRequestsResponse
{
    public List<PlayerInfo> requests;
}

public class FriendsResponse
{
    public List<PlayerInfo> friends;
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

public enum Respond
{
    Accept,
    Reject
}

public class FriendRequestReceivedResponse 
{
    public PlayerInfo from;
}