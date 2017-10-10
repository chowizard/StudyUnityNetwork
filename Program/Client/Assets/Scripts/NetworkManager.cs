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

            if(Input.GetKeyDown(KeyCode.Escape))
            {

            }
        }
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
        NetworkServer.Listen(port);
        isAtStartup = false;

        state = State.Server;
        message = "Setup server.";
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        networkClient = new NetworkClient();
        networkClient.RegisterHandler(MsgType.Connect, OnConnected);
        networkClient.Connect(ip, port);
        isAtStartup = false;

        state = State.Client;
        message = "Setup client.";
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        networkClient = ClientScene.ConnectLocalServer();
        networkClient.RegisterHandler(MsgType.Connect, OnConnected);
        isAtStartup = false;

        state = State.LocalClient;
        message = "Setup local client.";
    }

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        message = "Connected to server";
        Debug.Log(message);
    }
}
