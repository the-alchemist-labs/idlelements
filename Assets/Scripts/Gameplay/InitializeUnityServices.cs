using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class InitializeUnityServices
{
    public InitializeUnityServices()
    {
        Init();
    }

    private async void Init()
    {
        await UnityServices.InitializeAsync();
        SignInAnonymously();
    }

    private async void SignInAnonymously()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
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
