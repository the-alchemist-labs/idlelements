using System.Collections.Generic;
using System.Threading.Tasks;
using SocketIOClient;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Player
{
    private static Player _instance;

    public string Id { get; private set; }
    public string FriendCode { get; private set; }
    public List<string> Friends { get; private set; }

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

        SocketIO.Instance.Socket.On("friend_request_response", OnFriendRequestResponse);

        GameEvents.PlayerInitilized();
    }

    private void OnFriendRequestResponse(SocketIOResponse response)
    {
        Debug.Log($"OnFriendRequestResponse: {response}");
    }

    public async Task<Status> SendFriendRequest(string code)
    {
        FriendRequestResponse res = await Http.Post<FriendRequestResponse>(
            $"{Consts.ServerURI}/friends/request/{Id}",
            // new Dictionary<string, string> { { "friendCode", code } }
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
