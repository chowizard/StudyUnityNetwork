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

    public GameObject playerPrefab;

    private static NetworkManager singleton;

    private SceneMain sceneMain;

    private ConnectionConfig connectionConfiguration;

    private NetworkControllerServer serverController;
    private NetworkControllerClient clientController;
    private NetworkControllerLocalClient localClientController;

    [HideInInspector]
    public string message;

    public static NetworkManager Singleton
    {
        get
        {
            return singleton;
        }
    }

    public void SetupResources()
    {
        playerPrefab = Resources.Load<GameObject>("Player");
        Debug.Assert(playerPrefab != null);

        ClientScene.RegisterPrefab(playerPrefab);
    }

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        sceneMain = transform.parent.GetComponent<SceneMain>();

        SetupConnectionConfiguration();
        SetupResources();
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

        GUI.Label(new Rect(2, 80, 600, 100), message);
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        if(serverController == null)
            serverController = new NetworkControllerServer(this);

        serverController.Setup();

        isAtStartup = false;
        mode = Mode.Server;
        message = "Setup server.";
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        if(clientController == null)
            clientController = new NetworkControllerClient(this);

        clientController.Setup();

        isAtStartup = false;
        mode = Mode.Client;
        message = "Setup client.";

        //Debug.Assert(networkClient.isConnected);
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        if(localClientController == null)
            localClientController = new NetworkControllerLocalClient(this);

        localClientController.Setup();

        isAtStartup = false;
        mode = Mode.LocalClient;
        message = "Setup local client.";
    }

    public CharacterEntity AddPlayerCharacter(int id)
    {
        CharacterEntity player = sceneMain.entityManager.CreatePlayerCharacter(playerPrefab);
        player.id = id;

        sceneMain.entityManager.AddPlayerCharacter(player.id, player);

        return player;
    }

    public void RemovePlayerCharacter(int id)
    {
        sceneMain.entityManager.RemovePlayerCharacter(id);
    }

    public NetworkControllerServer ServerController
    {
        get
        {
            return serverController;
        }
    }

    public NetworkControllerClient ClientController
    {
        get
        {
            return clientController;
        }
    }

    public NetworkControllerLocalClient LocalClientController
    {
        get
        {
            return localClientController;
        }
    }

    private void SetupConnectionConfiguration()
    {
        if(connectionConfiguration == null)
            connectionConfiguration = new ConnectionConfig();

        //connectionConfiguration.AddChannel(QosType.Reliable);
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
        if(serverController != null)
        {
            serverController.Terminate();
            serverController = null;
        }

        message = "Server was terminate.";
        Debug.Log(message);
    }

    private void TerminateAtClient()
    {
        if(clientController != null)
        {
            clientController.Terminate();
            clientController = null;
        }

        message = "Client was terminate.";
        Debug.Log(message);
    }

    private void TerminateAtLocalClient()
    {
        if(localClientController != null)
        {
            localClientController.Terminate();
            localClientController = null;
        }

        message = "Local Client was terminate.";
        Debug.Log(message);
    }
}
