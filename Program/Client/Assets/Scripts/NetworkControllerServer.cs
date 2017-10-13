using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

public sealed class NetworkControllerServer
{
    private NetworkManager networkManager;
    private Dictionary<int, NetworkClient> networkClients = new Dictionary<int, NetworkClient>();

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

        NetworkServer.Listen(networkManager.port);
    }

    public void Terminate()
    {
        NetworkServer.DisconnectAll();
        NetworkServer.Reset();
    }

    public NetworkClient GetNetworkClient(int connectionId)
    {
        NetworkClient data;
        return networkClients.TryGetValue(connectionId, out data) ? data : null;
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
        AddNetworkClient(networkMessage.conn);

        string message = string.Format("Connected from client. (Connection ID = {0}    Address = {1})",
                                       networkMessage.conn.connectionId,
                                       networkMessage.conn.address);
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnDisconnected(NetworkMessage networkMessage)
    {
        RemoveNetworkClient(networkMessage.conn.connectionId);

        string message = "Client was disconnected.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public NetworkClient[] NetworkClients
    {
        get
        {
            return (networkClients.Count > 0) ? networkClients.Values.ToArray() : null;
        }
    }

    public int NetworkClientCount
    {
        get
        {
            return networkClients.Count;
        }
    }

    private NetworkClient AddNetworkClient(NetworkConnection connection)
    {
        if(connection == null)
            return null;

        NetworkClient networkClient = new NetworkClient(connection);

        RemoveNetworkClient(connection.connectionId);
        networkClients.Add(connection.connectionId, networkClient);

        return networkClient;
    }

    private bool RemoveNetworkClient(int connectionId)
    {
        return networkClients.Remove(connectionId);
    }

    private bool RemoveNetworkClient(NetworkClient networkClient)
    {
        if(networkClient == null)
            return false;

        if(networkClient.connection != null)
        {
            return RemoveNetworkClient(networkClient.connection.connectionId);
        }
        else
        {
            foreach(var pair in networkClients)
            {
                NetworkClient currentNetClient = pair.Value;
                if(currentNetClient == null)
                    continue;

                if(currentNetClient == networkClient)
                    return networkClients.Remove(pair.Key);
            }

            return false;
        }
    }
}
