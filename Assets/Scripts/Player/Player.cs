using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int MAX_LEVEL = 30;
    public static Player Instance { get; private set; }

    public string Id { get; private set; }
    public string FriendCode { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }
    public bool IsOnline { get; private set; }

    public Party Party { get; private set; }
    public PlayerResources Resources { get; private set; }
    public Friends Friends { get; private set; }
    public Inventory Inventory { get; private set; }

    async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            await Initialize();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private async Task Initialize()
    {
        await InitializeUnityServices();

        PlayerState state = DataService.Instance.LoadData<PlayerState>(FileName.PlayerState, true);
        PlayerInfo playerInfo = await PlayerApi.GetPlayer(AuthenticationService.Instance.PlayerId, true);

        Id = playerInfo.id;
        FriendCode = playerInfo.friendCode;
        Name = playerInfo.name;
        IsOnline = playerInfo.isOnline;
        
        Level = state.Level;
        Experience = state.Experience;
        Party = state.Party;
        Resources = state.Resources;
        Inventory = state.Inventory;
        gameObject.AddComponent<SocketIO>();

        Friends = await Friends.CreateAsync();

        SetEventListeners();

        GameEvents.PlayerInitialized();
    }

    private void SetEventListeners()
    {
        GameEvents.OnLevelUp += SavePlayerProgress;
        GameEvents.OnPartyUpdated += SavePlayerProgress;
        GameEvents.OnElementalCaught += SavePlayerProgress;
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

    public bool IsMaxLevel()
    {
        return Level == MAX_LEVEL;
    }

    public int ExpToLevelUp(int level)
    {
        return (int)(Math.Round((Math.Pow(level, 3) + level * 200) / 100.0) * 1000);
    }

    private bool ShouldToLevelUp()
    {
        return Experience >= ExpToLevelUp(Level);
    }

    public void GainExperience(int exp)
    {
        Experience += exp;

        while (ShouldToLevelUp() && !IsMaxLevel())
        {
            Experience -= ExpToLevelUp(Level);
            Level++;
            GameEvents.LevelUp();
        }
    }

    public PlayerInfo GetPlayerInfo()
    {
        return new PlayerInfo(
            Id,
            FriendCode,
            Name,
            Level,
            ElementalManager.Instance.elementalCaught,
            Instance.Party,
            IsOnline
        );
    }
}
