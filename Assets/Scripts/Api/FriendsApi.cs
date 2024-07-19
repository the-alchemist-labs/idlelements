using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
public class StatusResponse
{
    public Status status;
    public string message;
}

public static class FriendsApi
{
    public async static Task<List<PlayerInfo>> GetPendingFriendRequests()
    {
        PendingFriendRequestsResponse res = await Http.Get<PendingFriendRequestsResponse>($"{Consts.ServerURI}/friends/requests/pending/{Player.Instance.Id}");
        return res?.requests;
    }

    public async static Task<List<PlayerInfo>> GetFriends()
    {
        FriendsResponse res = await Http.Get<FriendsResponse>($"{Consts.ServerURI}/friends/{Player.Instance.Id}");
        return res?.friends;
    }

    public async static Task<StatusResponse> RespondToFriendRequest(string requestFrom, Respond respond)
    {
        return await Http.Post<StatusResponse>(
            $"{Consts.ServerURI}/friends/requests/respond/{Player.Instance.Id}",
            new { requestFrom = requestFrom, respond = respond }
        );
    }

    public async static Task<StatusResponse> SendFriendRequest(string friendCode)
    {
        return await Http.Post<StatusResponse>(
            $"{Consts.ServerURI}/friends/requests/send/{Player.Instance.Id}",
            new { friendCode = friendCode }
        );
    }
}
