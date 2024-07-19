using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public static class GameEvents
{
    public static event Action OnSocketConnected;
    public static event Action OnMapDataChanged;
    public static event Action OnIdleGainsChanged;
    public static event Action OnElementalCaught;
    public static event Action OnTriggerElementalToast;
    public static event Action OnGoldUpdated;
    public static event Action OnEssenceUpdated;
    public static event Action OnLevelUp;
    public static event Action OnTokensUpdated;
    public static event Action OnPartyUpdated;
    public static event Action OnFriendsUpdated;
    public static event Action OnPlayerInitialized;

    public static void SocketConnected()
    {
        OnSocketConnected?.Invoke();
    }

    public static void MapDataChanged()
    {
        OnMapDataChanged?.Invoke();
    }

    public static void IdleGainsChanged()
    {
        OnIdleGainsChanged?.Invoke();
    }

    public static void ElementalCaught()
    {
        OnElementalCaught?.Invoke();

    }

    public static void TriggerElementalToast()
    {
        OnTriggerElementalToast?.Invoke();
    }

    public static void GoldUpdated()
    {
        OnGoldUpdated?.Invoke();
    }

    public static void EssenceUpdated()
    {
        OnEssenceUpdated?.Invoke();
    }

    public static void LevelUp()
    {
        OnLevelUp?.Invoke();
        Analytics.CustomEvent("LeveledUp", new Dictionary<string, object> { { "level", Player.Instance.Level } });
    }

    public static void TokensUpdated()
    {
        OnTokensUpdated?.Invoke();
    }

    public static void PartyUpdated()
    {
        OnPartyUpdated?.Invoke();
    }

    public static void FriendsUpdated()
    {
        OnFriendsUpdated?.Invoke();
    }

    public static void PlayerInitialized()
    {
        OnPlayerInitialized?.Invoke();
    }
}