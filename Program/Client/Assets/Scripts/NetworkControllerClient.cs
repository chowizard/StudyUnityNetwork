using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public sealed class NetworkControllerClient
{
    private NetworkManager networkManager;
    private SceneMain mainScene;

    private NetworkClient netClient;
    private PlayerComponentMove myPlayer;

    public NetworkControllerClient(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
        mainScene = networkManager.transform.parent.GetComponent<SceneMain>();
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
            netClient.RegisterHandler(MsgType.AddPlayer, OnAddPlayer);
            netClient.RegisterHandler(MsgType.RemovePlayer, OnRemovePlayer);
        }

        netClient.Connect(networkManager.ip, networkManager.port);
    }

    public void SpawnPlayer(short playerControllerId)
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        Debug.Assert(playerPrefab != null);

        ClientScene.RegisterPrefab(playerPrefab);

        GameObject myPlayerObject = Object.Instantiate<GameObject>(playerPrefab);
        myPlayerObject.name = playerPrefab.name;
        myPlayerObject.transform.position = playerPrefab.transform.position;
        myPlayerObject.transform.parent = mainScene.entityManager.transform;

        myPlayer = myPlayerObject.transform.GetComponent<PlayerComponentMove>();
    }

    public void Terminate()
    {
        if((netClient != null) && netClient.isConnected)
            netClient.Disconnect();

        netClient.Shutdown();
    }

    public NetworkClient NetClient
    {
        get
        {
            return netClient;
        }
    }

    #region Network Events
    public void OnError(NetworkMessage networkMessage)
    {
        string logText = "Error occured. : ";
        logText += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnConnected(NetworkMessage networkMessage)
    {
        if(!ClientScene.ready)
            ClientScene.Ready(netClient.connection);

        string logText = string.Format("Connected from client. Address = {0})", networkMessage.conn.address);
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnDisconnected(NetworkMessage networkMessage)
    {
        string logText = string.Format("Connected from client. Address = {0})", networkMessage.conn.address);
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnReady(NetworkMessage networkMessage)
    {
        string logText = "ready to send message to server.";
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnNotReady(NetworkMessage networkMessage)
    {
        string logText = "Not ready to send message to server.";
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.LogError(logText);

        networkManager.message = logText;
    }

    public void OnAddPlayer(NetworkMessage networkMessage)
    {
        AddPlayerMessage targetMessage = networkMessage.ReadMessage<AddPlayerMessage>();

        string logText = string.Format("Add player. (Player Controller ID : {0}", targetMessage.playerControllerId);
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;

        SpawnPlayer(targetMessage.playerControllerId);
        ClientScene.AddPlayer(targetMessage.playerControllerId);
    }

    public void OnRemovePlayer(NetworkMessage networkMessage)
    {
        RemovePlayerMessage targetMessage = networkMessage.ReadMessage<RemovePlayerMessage>();

        string message = string.Format("Add player. (Player Controller ID : {0}", targetMessage.playerControllerId);
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);

        networkManager.message = message;

        ClientScene.RemovePlayer(targetMessage.playerControllerId);
        UnspawnPlayer(targetMessage.playerControllerId);
    }
    #endregion

    private void UnspawnPlayer(short playerControllerId)
    {
        if(myPlayer != null)
        {
            GameObject.Destroy(myPlayer);
            myPlayer = null;
        }
    }
}
