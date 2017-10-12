using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public sealed class NetworkControllerClient
{
    private NetworkManager networkManager;
    private NetworkClient netClient;

    public NetworkControllerClient(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
    }

    public void Setup()
    {
        if(netClient == null)
        {
            netClient = new NetworkClient();
            netClient.RegisterHandler(MsgType.Error, OnError);
            netClient.RegisterHandler(MsgType.Connect, OnConnected);
            netClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
            netClient.RegisterHandler(MsgType.Ready, OnReady);
            netClient.RegisterHandler(MsgType.NotReady, OnNotReady);
        }

        netClient.Connect(networkManager.ip, networkManager.port);
    }

    public void Terminate()
    {
        if((netClient != null) && netClient.isConnected)
            netClient.Disconnect();
    }

    public NetworkClient NetClient
    {
        get
        {
            return netClient;
        }
    }

    public void OnError(NetworkMessage networkMessage)
    {
        string message = "Error occured. : ";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnConnected(NetworkMessage networkMessage)
    {
        string message = "Connected to server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnDisconnected(NetworkMessage networkMessage)
    {
        string message = "Disconnected from server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnReady(NetworkMessage networkMessage)
    {
        string message = "ready to send message to server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnNotReady(NetworkMessage networkMessage)
    {
        string message = "Not ready to send message to server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.LogError(message);

        networkManager.message = message;
    }
}
