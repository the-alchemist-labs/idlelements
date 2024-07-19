using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SocketIOClient;
using UnityEngine;

public static class SocketEventName
{
    public const string FriendRequestaccepted = "friend_request_accepted";
    public const string FriendRequestReceived = "friend_request_received";
}

public class SocketIO : MonoBehaviour
{
    public static SocketIO Instance { get; private set; }
    public SocketIOUnity Socket { get; private set; }

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

    void OnDestroy()
    {
        Instance.Disconnect();
    }

    private async Task Initialize()
    {
        Uri uri = new Uri($"ws://{Consts.ServerURI}");
        string playerId = Player.Instance.Id;

        if (string.IsNullOrEmpty(playerId))
        {
            throw new InvalidOperationException("Player ID cannot be null or empty.");
        }

        Socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string> { { "playerId", playerId } },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        Socket.OnReconnectError += OnReconnectError;

        await Socket.ConnectAsync();

        Debug.Log("Socket connected");
        GameEvents.SocketConnected();
    }

    private void OnReconnectError(object sender, Exception e)
    {
        Debug.LogError("Socket failed to connect");
    }

    public void Disconnect()
    {
        Socket.Disconnect();
        Socket.Dispose();
    }

    public void RegisterEvent<T>(string eventName, Action<T> handler)
    {
        Socket.On(eventName, response =>
        {
            T parsedResponse = JsonConvert.DeserializeObject<T[]>(response.ToString()).First();
            MainThreadDispatcher.Enqueue(() => handler(parsedResponse));
        });
    }

    public void RegisterEvent(string eventName, Action handler)
    {
        Socket.On(eventName, response => MainThreadDispatcher.Enqueue(handler));
        Socket.On(eventName, response =>
        {
            MainThreadDispatcher.Enqueue(() => handler());
        });
    }
}
