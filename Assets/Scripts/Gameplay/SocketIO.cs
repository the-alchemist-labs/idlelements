using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SocketIOClient;
using UnityEngine;

public static class SocketEventName
{
    public const string FriendRequestaccepted = "friend_request_accepted";
    public const string FriendRequestReceived = "friend_request_received";
}

public class SocketIO
{
    private static SocketIO _instance;
    private bool isInitialized = false;

    public SocketIOUnity Socket { get; private set; }

    public static SocketIO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SocketIO();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

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

        Socket.OnConnected += OnConnected;
        Socket.OnReconnectError += OnReconnectError;

        Socket.Connect();

        isInitialized = true;
    }

    private void OnConnected(object sender, EventArgs e)
    {
        Debug.Log("Socket connected");
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
