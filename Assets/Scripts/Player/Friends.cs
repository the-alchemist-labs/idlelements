using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class FriendRequestAcceptedResponse
{
    public string fromPlayerId;
}

public class FriendRequestReceivedResponse
{
    public PlayerInfo from;
}

public class FriendOnlineStatusResponse
{
    public string playerId;
    public bool isOnline;
}

public class Friends
{
    public List<PlayerInfo> PendingFriendRequestsList { get; private set; }
    public List<PlayerInfo> FriendsList { get; private set; }

    public Friends() { }


    public static async Task<Friends> CreateAsync()
    {
        Friends friends = new Friends();
        await friends.InitializeAsync();
        return friends;
    }

    public async Task InitializeAsync()
    {
        Friends friends = new Friends();
        PendingFriendRequestsList = await FriendsApi.GetPendingFriendRequests();
        FriendsList = await FriendsApi.GetFriends();

        SocketIO.Instance.RegisterEvent<FriendRequestReceivedResponse>(SocketEventName.FriendRequestReceived, OnFriendRequestReceived);
        SocketIO.Instance.RegisterEvent<FriendOnlineStatusResponse>(SocketEventName.FriendOnlineStatus, OnFriendOnlineStatus);
        SocketIO.Instance.RegisterEvent<FriendRequestAcceptedResponse>(SocketEventName.FriendRequestaccepted, OnFriendRequestAccepted);

    }


    private void OnFriendRequestReceived(FriendRequestReceivedResponse response)
    {
        if (!PendingFriendRequestsList.Contains(response.from))
        {
            PendingFriendRequestsList.Add(response.from);
            GameEvents.FriendsUpdated();
            NotificationManager.Instance.PostNotification($"Friend request received from {response.from}");
        }
    }

    private async void OnFriendRequestAccepted(FriendRequestAcceptedResponse response)
    {
        FriendsList = await FriendsApi.GetFriends();
        GameEvents.FriendsUpdated();
        NotificationManager.Instance.PostNotification($"{response.fromPlayerId} accepted your friend request");
    }

    private void OnFriendOnlineStatus(FriendOnlineStatusResponse response)
    {
        FriendsList.Where(f => f.id == response.playerId).ToList().ForEach(f => f.isOnline = response.isOnline);
        FriendsList = FriendsList.OrderByDescending(f => f.isOnline).ToList();

        GameEvents.FriendsUpdated();
    }

    public async Task<StatusResponse> FriendRequestRespond(string playerId, Respond respond)
    {
        StatusResponse res = await FriendsApi.RespondToFriendRequest(playerId, respond);
        if (res.status == Status.Failed)
        {
            Debug.LogError($"Failed to {respond} friend request from {playerId}");
            return res;
        }

        PendingFriendRequestsList = await FriendsApi.GetPendingFriendRequests();
        FriendsList = await FriendsApi.GetFriends();

        GameEvents.FriendsUpdated();
        Debug.Log($"Succesfully {respond}ed friend request from {playerId}");
        return res;
    }

    public async Task<StatusResponse> SendFriendRequest(string code)
    {
        return await FriendsApi.SendFriendRequest(code);
    }
}