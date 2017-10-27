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

    private Dictionary<int, NetworkConnection> connections = new Dictionary<int, NetworkConnection>();

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

    public NetworkConnection GetConnection(int connectionId)
    {
        NetworkConnection data;
        return connections.TryGetValue(connectionId, out data) ? data : null;
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
        AddConnection(networkMessage.conn);
        //ClientScene.Ready(networkMessage.conn);


        string logText = string.Format("Connected from client. Address = {0})", networkMessage.conn.address);
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnDisconnected(NetworkMessage networkMessage)
    {
        OnRemovePlayer(networkMessage);
        RemoveConnection(networkMessage.conn.connectionId);


        string logText = string.Format("Client was disconnected. Address = {0})", networkMessage.conn.address);
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnReady(NetworkMessage networkMessage)
    {
        NetworkServer.SetClientReady(networkMessage.conn);


        string logText = "Client is ready : " + networkMessage.conn.connectionId;
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnNotReady(NetworkMessage networkMessage)
    {
        NetworkServer.SetClientNotReady(networkMessage.conn);


        string logText = "Client is not ready : " + networkMessage.conn.connectionId;
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.LogError(logText);

        networkManager.message = logText;
    }

    public void OnAddPlayer(NetworkMessage networkMessage)
    {
        AddPlayerMessage targetMessage = networkMessage.ReadMessage<AddPlayerMessage>();

        CharacterEntity playerCharacter = networkManager.RegisterPlayerCharacter(networkMessage.conn.connectionId);
        NetworkServer.AddPlayerForConnection(networkMessage.conn, playerCharacter.gameObject, targetMessage.playerControllerId);
        NetworkIdentity networkIdentity = playerCharacter.GetComponent<NetworkIdentity>();
        networkIdentity.AssignClientAuthority(networkMessage.conn);

        //NetworkServer.SpawnWithClientAuthority()
        //foreach(var pair in connections)
        //{
        //    NetworkConnection connection = pair.Value;
        //    if(connection == null || !connection.isConnected)
        //        continue;

        //    if(connection.connectionId == networkMessage.conn.connectionId)
        //        continue;

        //    NetworkServer.SendToClient(networkMessage.conn.connectionId, MsgType.AddPlayer, targetMessage);
        //}
        //ClientScene.AddPlayer(targetMessage.playerControllerId);


        string logText = string.Format("Add player. (Player Controller ID : {0})", targetMessage.playerControllerId);
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnRemovePlayer(NetworkMessage networkMessage)
    {
        RemovePlayerMessage targetMessage = networkMessage.ReadMessage<RemovePlayerMessage>();

        networkManager.UnregisterPlayerCharacter(networkMessage.conn.connectionId);
        NetworkServer.DestroyPlayersForConnection(networkMessage.conn);

        //foreach(var pair in connections)
        //{
        //    NetworkConnection connection = pair.Value;
        //    if(connection == null || !connection.isConnected)
        //        continue;

        //    if(connection.connectionId == networkMessage.conn.connectionId)
        //        continue;

        //    NetworkServer.SendToClient(networkMessage.conn.connectionId, MsgType.RemovePlayer, targetMessage);
        //}
        //ClientScene.RemovePlayer(targetMessage.playerControllerId);


        string logText = string.Format("Remove player. (Player Controller ID : {0})", targetMessage.playerControllerId);
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }
    #endregion

    public NetworkConnection[] Connections
    {
        get
        {
            return (connections.Count > 0) ? connections.Values.ToArray() : null;
        }
    }

    public int ConnectionCount
    {
        get
        {
            return connections.Count;
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

    private void AddConnection(NetworkConnection connection)
    {
        if(connection == null)
            return;

        RemoveConnection(connection.connectionId);
        connections.Add(connection.connectionId, connection);
    }

    private bool RemoveConnection(int connectionId)
    {
        NetworkConnection connection = GetConnection(connectionId);
        if(connection == null)
            return false;

        if((connection != null) && connection.isConnected)
        {
            connection.Disconnect();
            connection.Dispose();
        }

        return connections.Remove(connectionId);
    }

    private bool RemoveConnection(NetworkClient networkClient)
    {
        if(networkClient == null)
            return false;

        if(networkClient.connection != null)
        {
            return RemoveConnection(networkClient.connection.connectionId);
        }
        else
        {
            foreach(var pair in connections)
            {
                NetworkConnection currentConnection = pair.Value;
                if(currentConnection == null)
                    continue;

                if(currentConnection == networkClient.connection)
                    return connections.Remove(pair.Key);
            }

            return false;
        }
    }
}
