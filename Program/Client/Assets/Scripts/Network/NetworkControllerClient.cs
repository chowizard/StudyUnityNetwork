using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public sealed class NetworkControllerClient
{
    private NetworkManager networkManager;
    private SceneMain sceneMain;

    private NetworkClient netClient;

    public NetworkControllerClient(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
        sceneMain = networkManager.transform.parent.GetComponent<SceneMain>();
    }

    public void Setup()
    {
        SetupClient();
    }

    public void Terminate()
    {
        if(netClient == null)
            return;

        if(netClient.connection != null)
        {
            /* 플레이어가 있다면 플레이어를 제거한다. */
            CharacterEntity myCharacter = EntityManager.Instance.MyCharacter;
            if(myCharacter != null)
            {
                //EntityManager.Instance.RemoveEntity(myCharacter.netId.Value);
                EntityManager.Instance.DestroyEntity(myCharacter);
            }
        }

        if(netClient.isConnected)
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

        string logText = string.Format("Connected to server. Address = {0})", networkMessage.conn.address);
        logText += "\n[Connection] : " + networkMessage.conn;
        logText += "\n[Message Type] : " + networkMessage.msgType;
        Debug.Log(logText);

        networkManager.message = logText;
    }

    public void OnDisconnected(NetworkMessage networkMessage)
    {
        string logText = string.Format("Disconnected from server. Address = {0})", networkMessage.conn.address);
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
    #endregion

    private void SetupClient()
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
}
