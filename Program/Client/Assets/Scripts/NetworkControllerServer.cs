using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public sealed class NetworkControllerServer
{
    private NetworkManager networkManager;
    private Dictionary<int, NetworkClient> networkClients;

    public NetworkControllerServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
    }

    public void Setup()
    {
        NetworkServer.RegisterHandler(MsgType.Error, OnError);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnected);

        NetworkServer.Listen(networkManager.port);
    }

    public void Terminate()
    {
        NetworkServer.DisconnectAll();
        NetworkServer.Reset();
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
        string message = string.Format("Connected from client. (Connection ID = {0}    Address = {1})",
                                       networkMessage.conn.connectionId,
                                       networkMessage.conn.address);
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnDisconnected(NetworkMessage networkMessage)
    {
        string message = "Client was disconnected.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }
}
