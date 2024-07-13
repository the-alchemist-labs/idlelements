using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocketIOClient;
using Unity.Services.Authentication;
using UnityEngine;

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
                _instance.Initialize(AuthenticationService.Instance.PlayerId);
            }
            return _instance;
        }
    }

    public void Initialize(string playerId)
    { 
        if (isInitialized)
        {
            return;
        }

        Uri uri = new Uri(Consts.SocketUri);

        if (string.IsNullOrEmpty(playerId))
        {
            throw new InvalidOperationException("Player ID cannot be null or empty.");
        }

        Socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
            {
                {"id", playerId },
                {"name", playerId },
            },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket

        });


        Socket.OnConnected += OnConnected;
        Socket.OnReconnectError += OnReconnectError;

        Socket.Connect();

        // Socket.On("friend_request_response", OnFriendRequestResponse);

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

    // private void OnFriendRequestResponse(SocketIOResponse response)
    // {
    //     Debug.Log($"OnFriendRequestResponse: {response}");
    // }

    public void Disconnect()
    {
        Socket.Disconnect();
        Socket.Dispose();
    }
}
