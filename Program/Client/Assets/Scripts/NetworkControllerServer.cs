using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public sealed class NetworkControllerServer
{
    private NetworkManager networkManager;
    private SceneMain mainScene;

    private Dictionary<int, NetworkClient> remoteNetClients = new Dictionary<int, NetworkClient>();

    public NetworkControllerServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
        mainScene = networkManager.transform.parent.GetComponent<SceneMain>();
    }

    public void Setup()
    {
        SetupServer();

        // 로컬 클라이언트를 굳이 만들어야 할 필요는 없는 것 같다.
        //SetupLocalClient();
    }

    public void Terminate()
    {
        // 서버를 아예 종료하고 싶으면 그냥 Shutdown()을 호출하면 되는 것 같다.
        // 서버를 끄지 않고 뭔가 연결을 재설정하고 싶을 때나 이 함수들을 쓰면 될 것 같다.
        //NetworkServer.DisconnectAll();
        //NetworkServer.Reset();
        //NetworkServer.ResetConnectionStats();

        if(NetworkServer.active)
            NetworkServer.Shutdown();
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
        string message = string.Format("Connected from client. Address = {0})", networkMessage.conn.address);
        message += "\n[Connection] : " + networkMessage.conn;
        message += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        //ClientScene.Ready(networkMessage.conn);
        //AddRemoteNetworkClient(networkMessage.conn);
    }

    public void OnDisconnected(NetworkMessage networkMessage)
    {
        string message = string.Format("Client was disconnected. Address = {0})", networkMessage.conn.address);
        message += "\n[Connection] : " + networkMessage.conn;
        message += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        //RemoveRemoteNetworkClient(networkMessage.conn.connectionId);
    }

    public void OnReady(NetworkMessage networkMessage)
    {
        string message = "Client is ready : " + networkMessage.conn.connectionId;
        message += "\n[Connection] : " + networkMessage.conn;
        message += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        NetworkServer.SetClientReady(networkMessage.conn);
    }

    public void OnNotReady(NetworkMessage networkMessage)
    {
        string message = "Client is not ready : " + networkMessage.conn.connectionId;
        message += "\n[Connection] : " + networkMessage.conn;
        message += "\n[Message Type] : " + networkMessage.msgType;
        Debug.LogError(message);

        networkManager.message = message;

        NetworkServer.SetClientNotReady(networkMessage.conn);
    }

    public void OnAddPlayer(NetworkMessage networkMessage)
    {
        AddPlayerMessage targetMessage = networkMessage.ReadMessage<AddPlayerMessage>();

        string message = string.Format("Add player. (Player Controller ID : {1})", targetMessage.playerControllerId);
        message += "\n[Connection] : " + networkMessage.conn;
        message += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        //ClientScene.AddPlayer(targetMessage.playerControllerId);
    }

    public void OnRemovePlayer(NetworkMessage networkMessage)
    {
        RemovePlayerMessage targetMessage = networkMessage.ReadMessage<RemovePlayerMessage>();

        string message = string.Format("Remove player. (Player Controller ID : {1})", targetMessage.playerControllerId);
        message += "\n[Connection] : " + networkMessage.conn;
        message += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        //ClientScene.RemovePlayer(targetMessage.playerControllerId);
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
        string message = string.Format("Remote client was disconnected from server. (Connection ID = {0}    Address = {1})",
                                       networkMessage.conn.connectionId,
                                       networkMessage.conn.address);
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        RemoveRemoteNetworkClient(networkMessage.conn.connectionId);
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

    public void OnAddPlayerRemoteClient(NetworkMessage networkMessage)
    {
        AddPlayerMessage targetMessage = networkMessage.ReadMessage<AddPlayerMessage>();

        string message = string.Format("Add player in remote client. (Connection ID : {0}    Player Controller ID : {1}",
                                       networkMessage.conn.connectionId,
                                       targetMessage.playerControllerId);
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        ClientScene.AddPlayer(targetMessage.playerControllerId);
    }

    public void OnRemovePlayerRemoteClient(NetworkMessage networkMessage)
    {
        RemovePlayerMessage targetMessage = networkMessage.ReadMessage<RemovePlayerMessage>();

        string message = string.Format("Remove player from remote client. (Connection ID : {0}    Player Controller ID : {1}",
                                       networkMessage.conn.connectionId,
                                       targetMessage.playerControllerId);
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        ClientScene.RemovePlayer(targetMessage.playerControllerId);
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
        Terminate();

        NetworkServer.RegisterHandler(MsgType.Error, OnError);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        NetworkServer.RegisterHandler(MsgType.Ready, OnReady);
        NetworkServer.RegisterHandler(MsgType.NotReady, OnNotReady);
        NetworkServer.RegisterHandler(MsgType.AddPlayer, OnAddPlayer);
        NetworkServer.RegisterHandler(MsgType.RemovePlayer, OnRemovePlayer);

        NetworkServer.Listen(networkManager.port);
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
        netClient.RegisterHandler(MsgType.AddPlayer, OnAddPlayerRemoteClient);
        netClient.RegisterHandler(MsgType.RemovePlayer, OnRemovePlayerRemoteClient);

        RemoveRemoteNetworkClient(connection.connectionId);
        remoteNetClients.Add(connection.connectionId, netClient);

        return netClient;
    }

    private bool RemoveRemoteNetworkClient(int connectionId)
    {
        NetworkClient remoteNetworkClient = GetRemoteNetworkClient(connectionId);
        if(remoteNetworkClient == null)
            return false;

        if((remoteNetworkClient != null) && remoteNetworkClient.isConnected)
            remoteNetworkClient.Disconnect();

        remoteNetworkClient.Shutdown();

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
