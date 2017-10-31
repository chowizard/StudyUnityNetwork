using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using UnityNet.Client.Core;

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

    public Dictionary<string, GameObject> spawningPrefabs = new Dictionary<string, GameObject>();

    private ConnectionConfig connectionConfiguration;

    private NetworkControllerServer serverController;
    private NetworkControllerClient clientController;
    private NetworkControllerLocalClient localClientController;

    [HideInInspector]
    public string message;

    public static NetworkManager Instance
    {
        get
        {
            return SceneMain.Singleton.NetworkManager;
        }
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

    public CharacterEntity RegisterPlayerCharacter(int id)
    {
        return RegisterPlayerCharacter(id, Vector3.zero, Quaternion.identity);
    }

    public CharacterEntity RegisterPlayerCharacter(int id, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetSpawningPrefab(Defines.SpawningPrefab.Player);

        CharacterEntity entity = EntityManager.Instance.CreatePlayerCharacter(prefab, id);
        entity.id = id;

        EntityManager.Instance.AddEntity(entity.id, entity);

        return entity;
    }

    public CharacterEntity RegisterNonPlayerCharacter(int id)
    {
        return RegisterNonPlayerCharacter(id, Vector3.zero, Quaternion.identity);
    }

    public CharacterEntity RegisterNonPlayerCharacter(int id, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetSpawningPrefab(Defines.SpawningPrefab.Enemy);

        CharacterEntity entity = EntityManager.Instance.CreateNonPlayerCharacter(prefab, id, position, rotation);
        entity.id = id;

        EntityManager.Instance.AddEntity(entity.id, entity);

        return entity;
    }

    public void UnregisterPlayerCharacter(int id)
    {
        CharacterEntity player = EntityManager.Instance.RemoveEntity(id);
        EntityManager.Instance.DestroyEntity(player);
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

    private void Awake()
    {
    }

    private void Start()
    {
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

    private void SetupConnectionConfiguration()
    {
        if(connectionConfiguration == null)
            connectionConfiguration = new ConnectionConfig();

        //connectionConfiguration.AddChannel(QosType.Reliable);
    }

    private void SetupResources()
    {
        ClearSpawningPrefab();

        RegisterSpawningPrefab(Defines.SpawningPrefab.Player, "Entity/Player");
        RegisterSpawningPrefab(Defines.SpawningPrefab.Enemy, "Entity/Enemy");
        RegisterSpawningPrefab(Defines.SpawningPrefab.Bullet, "Entity/Bullet");
    }

    private void RegisterSpawningPrefab(string keyName, string resourcePath)
    {
        GameObject prefab = Resources.Load<GameObject>(resourcePath);
        Debug.Assert(prefab != null);

        AddSpawningPrefab(keyName, prefab);
        ClientScene.RegisterPrefab(prefab);
    }

    private void UnregisterSpawningPrefab(string keyName)
    {
        GameObject prefab = GetSpawningPrefab(keyName);
        if(prefab == null)
            return;

        ClientScene.UnregisterPrefab(prefab);
        RemoveSpawningPrefab(keyName);
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

    private void ClearSpawningPrefab()
    {
        spawningPrefabs.Clear();
    }

    private void AddSpawningPrefab(string keyName, GameObject prefab)
    {
        Debug.Assert(string.IsNullOrEmpty(keyName) == false);
        Debug.Assert(prefab != null);
        if(prefab == null)
            return;

        spawningPrefabs.Add(keyName, prefab);
    }

    private void RemoveSpawningPrefab(string keyName)
    {
        Debug.Assert(string.IsNullOrEmpty(keyName) == false);

        spawningPrefabs.Remove(keyName);
    }

    public GameObject GetSpawningPrefab(string keyName)
    {
        Debug.Assert(string.IsNullOrEmpty(keyName) == false);

        GameObject data;
        return spawningPrefabs.TryGetValue(keyName, out data) ? data : null;
    }
}
