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
    public string name;
    public int level;
    public int elementalsCaught;
    public List<int> party;

    public PlayerInfo(string id, string friendCode, string name, int level, int elementalsCaught, List<int> party)
    {
        this.id = id;
        this.friendCode = friendCode;
        this.name = name;
        this.level = level;
        this.elementalsCaught = elementalsCaught;
        this.party = party;
    }
}

public class Player
{
    private static Player _instance;

    public string Id { get; private set; }
    public string FriendCode { get; private set; }
    public string Name { get; private set; }
    public Friends Friends { get; private set; }

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

        PlayerInfo playerInfo = await PlayerApi.GetPlayer(AuthenticationService.Instance.PlayerId, true);

        Id = playerInfo.id;
        FriendCode = playerInfo.friendCode;
        Name = $"Player_{FriendCode?.ToLower()}";

        SocketIO.Instance.Initialize();

        GameEvents.OnSocketConnected += InitializeFriends;
        GameEvents.OnLevelUp += SavePlayerProgress;
        GameEvents.OnPartyUpdated += SavePlayerProgress;
        GameEvents.OnElementalCaught += SavePlayerProgress;
    }

    public PlayerInfo GetPlayerInfo()
    {
        return new PlayerInfo(
            Id,
            FriendCode,
            Name,
            State.level,
            State.Elementals.elementalCaught,
            new List<int> { (int)State.party.First, (int)State.party.Second, (int)State.party.Third }
        );
    }

    private void InitializeFriends()
    {
        Friends = new Friends();
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

    private async void SavePlayerProgress()
    {
        PlayerInfo playerInfo = GetPlayerInfo();
        await PlayerApi.UpdatePlayerInfo(playerInfo);
    }
}
