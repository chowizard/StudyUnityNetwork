using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

public sealed class NetworkControllerServer
{
    private NetworkManager networkManager;
    private NetworkClient localNetClient;
    private Dictionary<int, NetworkClient> remoteNetClients = new Dictionary<int, NetworkClient>();

    public NetworkControllerServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
    }

    public void Setup()
    {
        NetworkServer.Reset();

        NetworkServer.RegisterHandler(MsgType.Error, OnError);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        //NetworkServer.RegisterHandler(MsgType.Ready, OnReady);
        //NetworkServer.RegisterHandler(MsgType.NotReady, OnNotReady);

        NetworkServer.Listen(networkManager.port);
    }

    public void Terminate()
    {
        NetworkServer.DisconnectAll();
        NetworkServer.Reset();
    }

    public NetworkClient GetRemoteNetworkClient(int connectionId)
    {
        NetworkClient data;
        return remoteNetClients.TryGetValue(connectionId, out data) ? data : null;
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
        AddRemoteNetworkClient(networkMessage.conn);
        //if(!ClientScene.ready)
        //    ClientScene.Ready(networkMessage.conn);

        string message = string.Format("Connected from client. (Connection ID = {0}    Address = {1})",
                                       networkMessage.conn.connectionId,
                                       networkMessage.conn.address);
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnDisconnected(NetworkMessage networkMessage)
    {
        RemoveRemoteNetworkClient(networkMessage.conn.connectionId);

        string message = "Client was disconnected.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnConnectedClient(NetworkMessage networkMessage)
    {
        string message = "Connected to server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnDisconnectedClient(NetworkMessage networkMessage)
    {
        string message = "Disconnected from server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnReady(NetworkMessage networkMessage)
    {
        string message = "Client is ready : " + networkMessage.conn.connectionId;
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnNotReady(NetworkMessage networkMessage)
    {
        string message = "Client is not ready : " + networkMessage.conn.connectionId;
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.LogError(message);

        networkManager.message = message;
    }

    public NetworkClient[] NetworkClients
    {
        get
        {
            return (remoteNetClients.Count > 0) ? remoteNetClients.Values.ToArray() : null;
        }
    }

    public int NetworkClientCount
    {
        get
        {
            return remoteNetClients.Count;
        }
    }

    private NetworkClient AddRemoteNetworkClient(NetworkConnection connection)
    {
        if(connection == null)
            return null;

        NetworkClient netClient = new NetworkClient(connection);
        netClient.RegisterHandler(MsgType.Error, OnError);
        netClient.RegisterHandler(MsgType.Connect, OnConnectedClient);
        netClient.RegisterHandler(MsgType.Disconnect, OnDisconnectedClient);
        netClient.RegisterHandler(MsgType.Ready, OnReady);
        netClient.RegisterHandler(MsgType.NotReady, OnNotReady);

        RemoveRemoteNetworkClient(connection.connectionId);
        remoteNetClients.Add(connection.connectionId, netClient);

        return netClient;
    }

    private bool RemoveRemoteNetworkClient(int connectionId)
    {
        return remoteNetClients.Remove(connectionId);
    }

    private bool RemoveRemoteNetworkClient(NetworkClient networkClient)
    {
        if(networkClient == null)
            return false;

        if(networkClient.connection != null)
        {
            return RemoveRemoteNetworkClient(networkClient.connection.connectionId);
        }
        else
        {
            foreach(var pair in remoteNetClients)
            {
                NetworkClient currentNetClient = pair.Value;
                if(currentNetClient == null)
                    continue;

                if(currentNetClient == networkClient)
                    return remoteNetClients.Remove(pair.Key);
            }

            return false;
        }
    }
}
