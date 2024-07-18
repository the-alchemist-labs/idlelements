using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

[Serializable]
public class PlayerInfo
{
    public string id;
    public string friendCode;

    public string name { get { return $"Player_{friendCode.ToLower()}"; } }
}

public class Player
{
    private static Player _instance;

    public string Id { get; private set; }
    public string FriendCode { get; private set; }
    public List<PlayerInfo> PendingFriendRequests { get; private set; }
    public List<PlayerInfo> Friends { get; private set; }

    private Player() { }

    public static Player Instance
    {
        get
        {
            if (_instance?.Id == null)
            {
                _instance = new Player();
                _ = _instance.Initialize();
            }
            return _instance;
        }
    }

    public async Task Initialize()
    {
        if (_instance?.Id != null)
        {
            return;
        }

        await InitializeUnityServices();
        Id = AuthenticationService.Instance.PlayerId;
        FriendCode = (await Http.Get<FriendCodeResponse>($"{Consts.ServerURI}/players/code/{Id}"))?.code;
        PendingFriendRequests = (await Http.Get<PendingFriendRequestsResponse>($"{Consts.ServerURI}/friends/requests/pending/{Id}"))?.requests;
        Friends = (await Http.Get<FriendsResponse>($"{Consts.ServerURI}/friends/{Id}"))?.friends;

        SocketIO.Instance.RegisterEvent<FriendRequestReceivedResponse>(SocketEventName.FriendRequestReceived, OnFriendRequestReceived);
        SocketIO.Instance.RegisterEvent(SocketEventName.FriendRequestaccepted, OnFriendRequestAccepted);

        GameEvents.PlayerInitilized();
    }

    private void OnFriendRequestReceived(FriendRequestReceivedResponse response)
    {
        if (!PendingFriendRequests.Contains(response.from))
        {
            PendingFriendRequests.Add(response.from);
            GameEvents.FriendsUpdated();
        }
    }

    private async void OnFriendRequestAccepted()
    {
        Friends = (await Http.Get<FriendsResponse>($"{Consts.ServerURI}/friends/{Id}"))?.friends;
        GameEvents.FriendsUpdated();
    }

    public async Task<Status> FriendRequestRespond(string playerId, Respond respond)
    {
        Status res = await Http.Post<Status>($"{Consts.ServerURI}/friends/requests/respond/{Id}", new { requestFrom = playerId, respond = respond });
        if (res == Status.Failed)
        {
            Debug.LogError($"Failed to {respond} friend request from {playerId}");
            return Status.Failed;
        }

        PendingFriendRequests = (await Http.Get<PendingFriendRequestsResponse>($"{Consts.ServerURI}/friends/requests/pending/{Id}"))?.requests;
        Friends = (await Http.Get<FriendsResponse>($"{Consts.ServerURI}/friends/{Id}"))?.friends;

        GameEvents.FriendsUpdated();
        Debug.Log($"Succesfully {respond}ed friend request from {playerId}");
        return Status.Success;
    }

    public async Task<Status> SendFriendRequest(string code)
    {
        FriendRequestResponse res = await Http.Post<FriendRequestResponse>(
            $"{Consts.ServerURI}/friends/requests/send/{Id}",
            new { friendCode = code }
            );
        return res?.status ?? Status.Failed;
    }

    private async Task InitializeUnityServices()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"Failed to sign in anonymously: {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Request failed: {ex.Message}");
        }
    }
}
