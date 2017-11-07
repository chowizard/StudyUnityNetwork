using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using UnityNet.Client.Core;

public class NetworkManager : MonoBehaviour
{
    public enum eMode
    {
        None = 0,

        Server,
        Client
    }



    public string ip = "127.0.0.1";
    public ushort port = 15632;
    public bool isAtStartup = true;
    public eMode mode = eMode.None;

    public Dictionary<string, GameObject> spawningPrefabs = new Dictionary<string, GameObject>();

    private ConnectionConfig connectionConfiguration;

    private NetworkControllerServer serverController;
    private NetworkControllerClient clientController;

    [HideInInspector]
    public string message;

    public static NetworkManager Instance
    {
        get
        {
            return GameManager.Singleton.networkManager;
        }
    }

    public void Terminate()
    {
        if(isAtStartup == false)
            return;

        switch(mode)
        {
        case eMode.Server:
            TerminateAtServer();
            break;

        case eMode.Client:
            TerminateAtClient();
            break;
        }

        mode = eMode.None;
        isAtStartup = true;
    }

    // Create a server and listen on a port
    public void StartByServer()
    {
        if(isAtStartup == false)
            return;

        if(serverController == null)
            serverController = new NetworkControllerServer(this);

        serverController.Setup();

        isAtStartup = false;
        mode = eMode.Server;
        message = "Setup server.";
    }

    // Create a client and connect to the server port
    public void StartByClient()
    {
        if(isAtStartup == false)
            return;

        if(clientController == null)
            clientController = new NetworkControllerClient(this);

        clientController.Setup();

        isAtStartup = false;
        mode = eMode.Client;
        message = "Setup client.";
    }

    public CharacterEntity RegisterPlayerCharacter(int ownerNetConnectionId, short playerControllId)
    {
        return RegisterPlayerCharacter(ownerNetConnectionId, playerControllId, Vector3.zero, Quaternion.identity);
    }

    public CharacterEntity RegisterPlayerCharacter(int ownerNetConnectionId,
                                                   short playerControllId,
                                                   Vector3 position,
                                                   Quaternion rotation)
    {
        GameObject prefab = GetSpawningPrefab(Defines.SpawningPrefab.PlayerCharacter);

        CharacterEntity entity = EntityManager.Instance.CreatePlayerCharacter(prefab, ownerNetConnectionId, playerControllId);

        return entity;
    }

    public CharacterEntity RegisterNonPlayerCharacter()
    {
        return RegisterNonPlayerCharacter(Vector3.zero, Quaternion.identity);
    }

    public CharacterEntity RegisterNonPlayerCharacter(Vector3 position, Quaternion rotation)
    {
        GameObject prefab = GetSpawningPrefab(Defines.SpawningPrefab.NonPlayerCharacter);

        CharacterEntity entity = EntityManager.Instance.CreateNonPlayerCharacter(prefab, position, rotation);

        return entity;
    }

    public void UnregisterPlayerCharacter(int ownerNetConnectionId)
    {
        CharacterEntity[] myControlledEntities = EntityManager.Instance.GetMyControlledEntities(ownerNetConnectionId);
        if(myControlledEntities == null)
            return;

        foreach(CharacterEntity myEntity in myControlledEntities)
        {
            if(myEntity == null)
                continue;

            //EntityManager.Instance.RemoveEntity(myEntity.netId.Value);
            EntityManager.Instance.DestroyEntity(myEntity);
        }
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
        if(Input.GetKeyDown(KeyCode.Escape))
            Terminate();
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

        RegisterSpawningPrefab(Defines.SpawningPrefab.PlayerCharacter, "Entity/PlayerCharacter");
        RegisterSpawningPrefab(Defines.SpawningPrefab.NonPlayerCharacter, "Entity/NonPlayerCharacter");
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
