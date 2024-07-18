using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FriendRequestReceivedResponse
{
    public PlayerInfo from;
}

public class Friends
{
    public List<PlayerInfo> PendingFriendRequestsList { get; private set; }
    public List<PlayerInfo> FriendsList { get; private set; }

    public Friends()
    {
        MainThreadDispatcher.Enqueue(() => Initialize());
    }

    public async void Initialize()
    {
        PendingFriendRequestsList = await FriendsApi.GetPendingFriendRequests(Player.Instance.Id);
        FriendsList = await FriendsApi.GetFriends();

        SocketIO.Instance.RegisterEvent<FriendRequestReceivedResponse>(SocketEventName.FriendRequestReceived, OnFriendRequestReceived);
        SocketIO.Instance.RegisterEvent(SocketEventName.FriendRequestaccepted, OnFriendRequestAccepted);

        GameEvents.FriendsUpdated();
    }


    private void OnFriendRequestReceived(FriendRequestReceivedResponse response)
    {
        if (!PendingFriendRequestsList.Contains(response.from))
        {
            PendingFriendRequestsList.Add(response.from);
            GameEvents.FriendsUpdated();
        }
    }

    private async void OnFriendRequestAccepted()
    {
        FriendsList = await FriendsApi.GetFriends();
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

        PendingFriendRequestsList = await FriendsApi.GetPendingFriendRequests(Player.Instance.Id);
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