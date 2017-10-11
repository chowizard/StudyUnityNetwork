using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public enum Mode
    {
        None = 0,

        Server,
        Client,
        LocalClient
    }

    public string ip = "127.0.0.1";

    public ushort port = 15632;

    public bool isAtStartup = true;

    public Mode mode = Mode.None;

    private NetworkClient netClient;

    private string message;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if(isAtStartup)
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                SetupServer();
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                SetupClient();
            }

            if(Input.GetKeyDown(KeyCode.B))
            {
                SetupServer();
                SetupLocalClient();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
            Terminate();
    }

    private void OnGUI()
    {
        if(isAtStartup)
        {
            GUI.Label(new Rect(2, 10, 150, 100), "Press S for server");
            GUI.Label(new Rect(2, 30, 150, 100), "Press B for both");
            GUI.Label(new Rect(2, 50, 150, 100), "Press C for client");
        }

        GUI.Label(new Rect(2, 80, 400, 100), message);
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        NetworkServer.RegisterHandler(MsgType.Error, OnError);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnectedFromClient);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnectedFromClient);

        NetworkServer.Listen(port);
        isAtStartup = false;

        //DontDestroyOnLoad(this);

        mode = Mode.Server;
        message = "Setup server.";
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        netClient = new NetworkClient();
        netClient.RegisterHandler(MsgType.Error, OnError);
        netClient.RegisterHandler(MsgType.Connect, OnConnectedToServer);
        netClient.RegisterHandler(MsgType.Disconnect, OnDisconnectedToServer);
        netClient.RegisterHandler(MsgType.Ready, OnReady);
        netClient.RegisterHandler(MsgType.NotReady, OnNotReady);

        netClient.Connect(ip, port);
        isAtStartup = false;

        mode = Mode.Client;
        message = "Setup client.";

        //Debug.Assert(networkClient.isConnected);
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        netClient = ClientScene.ConnectLocalServer();
        netClient.RegisterHandler(MsgType.Error, OnError);
        netClient.RegisterHandler(MsgType.Connect, OnConnectedToServer);
        netClient.RegisterHandler(MsgType.Disconnect, OnDisconnectedToServer);
        netClient.RegisterHandler(MsgType.Ready, OnReady);
        netClient.RegisterHandler(MsgType.NotReady, OnNotReady);

        isAtStartup = false;

        mode = Mode.LocalClient;
        message = "Setup local client.";
    }

    public void OnConnectedFromClient(NetworkMessage networkMessage)
    {
        //netClient = networkMessage.ReadMessage<NetworkClient>();

        message = "Connected from client.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);
    }

    public void OnDisconnectedFromClient(NetworkMessage networkMessage)
    {
        message = "Client was disconnected.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);
    }

    public void OnConnectedToServer(NetworkMessage networkMessage)
    {
        message = "Connected to server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);
    }

    public void OnDisconnectedToServer(NetworkMessage networkMessage)
    {
        message = "Disconnected from server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);
    }

    public void OnError(NetworkMessage networkMessage)
    {
        message = "Error occured. : ";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);
    }

    public void OnReady(NetworkMessage networkMessage)
    {
        message = "ready to send message to server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.Log(message);
    }

    public void OnNotReady(NetworkMessage networkMessage)
    {
        message = "Not ready to send message to server.";
        message += "\nMessage Type : " + networkMessage.msgType;
        Debug.LogError(message);
    }

    public NetworkClient NetClient
    {
        get
        {
            return netClient;
        }
    }

    private void Terminate()
    {
        switch(mode)
        {
        case Mode.Server:
            TerminateAtServer();
            break;

        case Mode.Client:
            TerminateAtClient();
            break;

        case Mode.LocalClient:
            TerminateAtLocalClient();
            break;
        }

        isAtStartup = true;
    }

    private void TerminateAtServer()
    {
        NetworkServer.DisconnectAll();
        NetworkServer.Reset();

        message = "Server was terminate.";
        Debug.Log(message);
    }

    private void TerminateAtClient()
    {
        if(netClient.isConnected)
            netClient.Disconnect();

        message = "Client was terminate.";
        Debug.Log(message);
    }

    private void TerminateAtLocalClient()
    {
        if(netClient.isConnected)
            netClient.Disconnect();

        message = "Local Client was terminate.";
        Debug.Log(message);
    }
}
