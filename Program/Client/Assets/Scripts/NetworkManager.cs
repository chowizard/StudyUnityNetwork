using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public enum State
    {
        None = 0,

        Server,
        Client,
        LocalClient
    }

    public string ip = "127.0.0.1";

    public ushort port = 15632;

    public bool isAtStartup = true;

    public State state = State.None;

    private NetworkClient networkClient;

    private string message;

    // Use this for initialization
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
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnectedFromClient);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnectedFromClient);
        NetworkServer.RegisterHandler(MsgType.Error, OnError);

        NetworkServer.Listen(port);
        isAtStartup = false;

        //DontDestroyOnLoad(this);

        state = State.Server;
        message = "Setup server.";
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        networkClient = new NetworkClient();
        networkClient.RegisterHandler(MsgType.Connect, OnConnectedToServer);
        networkClient.RegisterHandler(MsgType.Disconnect, OnDisconnectedToServer);
        networkClient.RegisterHandler(MsgType.Error, OnError);

        networkClient.Connect(ip, port);
        isAtStartup = false;

        state = State.Client;
        message = "Setup client.";

        //Debug.Assert(networkClient.isConnected);
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        networkClient = ClientScene.ConnectLocalServer();
        networkClient.RegisterHandler(MsgType.Connect, OnConnectedToServer);
        networkClient.RegisterHandler(MsgType.Disconnect, OnDisconnectedToServer);
        networkClient.RegisterHandler(MsgType.Error, OnError);

        isAtStartup = false;

        state = State.LocalClient;
        message = "Setup local client.";
    }

    public void OnConnectedFromClient(NetworkMessage networkMessage)
    {
        message = "Connected from client.";
        Debug.Log(message);
    }

    public void OnDisconnectedFromClient(NetworkMessage networkMessage)
    {
        message = "Client was disconnected.";
        Debug.Log(message);
    }

    public void OnConnectedToServer(NetworkMessage networkMessage)
    {
        message = "Connected to server.";
        Debug.Log(message);
    }

    public void OnDisconnectedToServer(NetworkMessage networkMessage)
    {
        message = "Disconnected from server.";
        Debug.Log(message);
    }

    public void OnError(NetworkMessage networkMessage)
    {
        message = "Error occured. : ";
        Debug.Log(message);
    }

    private void Terminate()
    {
        switch(state)
        {
        case State.Server:
            TerminateAtServer();
            break;

        case State.Client:
            TerminateAtClient();
            break;

        case State.LocalClient:
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
        if(networkClient.isConnected)
            networkClient.Disconnect();

        message = "Client was terminate.";
        Debug.Log(message);
    }

    private void TerminateAtLocalClient()
    {
        if(networkClient.isConnected)
            networkClient.Disconnect();

        message = "Local Client was terminate.";
        Debug.Log(message);
    }
}
