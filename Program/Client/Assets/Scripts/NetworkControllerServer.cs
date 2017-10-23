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
        SetupServer();
        SetupLocalClient();
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

    #region Events For Server
    public void OnConnected(NetworkMessage networkMessage)
    {
        AddRemoteNetworkClient(networkMessage.conn);
        NetworkServer.SetClientReady(networkMessage.conn);

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
    #endregion


    #region Events For Local Client
    public void OnConnectedLocalClient(NetworkMessage networkMessage)
    {
        if(!ClientScene.ready)
            ClientScene.Ready(localNetClient.connection);

        string message = "Local client was connected to server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnDisconnectedLocalClient(NetworkMessage networkMessage)
    {
        string message = "Local client was disconnected from server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnReadyLocalClient(NetworkMessage networkMessage)
    {
        string message = "Local client is ready : " + networkMessage.conn.connectionId;
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnNotReadyLocalClient(NetworkMessage networkMessage)
    {
        string message = "Local client is not ready : " + networkMessage.conn.connectionId;
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.LogError(message);

        networkManager.message = message;
    }
    #endregion

    #region Events For Remote Client
    public void OnConnectedRemoteClient(NetworkMessage networkMessage)
    {
        string message = string.Format("Remote client was connected to server. (Connection ID = {0}    Address = {1})",
                                       networkMessage.conn.connectionId,
                                       networkMessage.conn.address);
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnDisconnectedRemoteClient(NetworkMessage networkMessage)
    {
        string message = "Remote client was disconnected from server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnReadyRemoteClient(NetworkMessage networkMessage)
    {
        string message = "Remote client is ready : " + networkMessage.conn.connectionId;
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;
    }

    public void OnNotReadyRemoteClient(NetworkMessage networkMessage)
    {
        string message = "Remote client is not ready : " + networkMessage.conn.connectionId;
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.LogError(message);

        networkManager.message = message;
    }
    #endregion

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

    private void SetupServer()
    {
        NetworkServer.Reset();

        NetworkServer.RegisterHandler(MsgType.Error, OnError);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        NetworkServer.RegisterHandler(MsgType.Ready, OnReady);
        NetworkServer.RegisterHandler(MsgType.NotReady, OnNotReady);

        NetworkServer.Listen(networkManager.port);
    }

    private void SetupLocalClient()
    {
        localNetClient = ClientScene.ConnectLocalServer();
        localNetClient.RegisterHandler(MsgType.Error, OnError);
        localNetClient.RegisterHandler(MsgType.Connect, OnConnectedLocalClient);
        localNetClient.RegisterHandler(MsgType.Disconnect, OnDisconnectedLocalClient);
        localNetClient.RegisterHandler(MsgType.Ready, OnReadyLocalClient);
        localNetClient.RegisterHandler(MsgType.NotReady, OnNotReadyLocalClient);
    }

    private NetworkClient AddRemoteNetworkClient(NetworkConnection connection)
    {
        if(connection == null)
            return null;

        NetworkClient netClient = new NetworkClient(connection);
        netClient.RegisterHandler(MsgType.Error, OnError);
        netClient.RegisterHandler(MsgType.Connect, OnConnectedRemoteClient);
        netClient.RegisterHandler(MsgType.Disconnect, OnDisconnectedRemoteClient);
        netClient.RegisterHandler(MsgType.Ready, OnReadyRemoteClient);
        netClient.RegisterHandler(MsgType.NotReady, OnNotReadyRemoteClient);

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
