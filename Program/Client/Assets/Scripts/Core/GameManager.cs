using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using UnityNet.Client.Core;

public class GameManager : MonoBehaviour
{
    public NetworkManager.eMode modeStartAt = NetworkManager.eMode.None;
    public int npcCount = 100;

    public CameraController mainCamera;

    public GameSceneManager gameSceneManager;
    public GameOptionManager gameOptionManager;
    public EntityManager entityManager;
    public NetworkManager networkManager;
    public UiManager uiManager;

    private static GameManager singleton;

    public static GameManager Singleton
    {
        get
        {
            return singleton;
        }
    }

    public void SpawnNonPlayerCharacters()
    {
        for(int count = 0; count < npcCount; ++count)
        {
            float positionX = Random.Range(-100.0f, 100.0f);
            float positionZ = Random.Range(-100.0f, 100.0f);
            Vector3 position = new Vector3(positionX, 0.0f, positionZ);

            float rotationY = Random.Range(0.0f, 360.0f);
            Quaternion rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);

            SpawnNonPlayerCharacter(position, rotation);
        }
    }

    private void Awake()
    {
        singleton = this;

        DontDestroyOnLoad(transform.root.gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(gameSceneManager != null);
        Debug.Assert(gameOptionManager != null);
        Debug.Assert(entityManager != null);
        Debug.Assert(networkManager != null);
        Debug.Assert(uiManager != null);

        switch(modeStartAt)
        {
        case NetworkManager.eMode.Server:
            {
                NetworkManager.Instance.mode = modeStartAt;
                NetworkManager.Instance.StartByServer();

                if(NetworkManager.Instance.isAtStartup == false)
                {
                    GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);
                    SpawnNonPlayerCharacters();
                }
            }
            break;

        case NetworkManager.eMode.Client:
            {
                NetworkManager.Instance.mode = modeStartAt;
                gameSceneManager.ChangeScene(GameScene.eSceneType.Start);
            }
            break;

        default:
            gameSceneManager.ChangeScene(GameScene.eSceneType.Intro);
            break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(networkManager.mode == NetworkManager.eMode.Server)
        {
            if(Input.GetKeyDown(KeyCode.X))
                SpawnNonPlayerCharacters();
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(networkManager.mode == NetworkManager.eMode.None)
                GenerateMyCharacter();
            if(networkManager.mode == NetworkManager.eMode.Client)
                SpawnMyCharacter();

        }
    }

    private void GenerateMyCharacter()
    {
        GameObject prefab = NetworkManager.Instance.GetSpawningPrefab(Defines.SpawningPrefab.PlayerCharacter);

        CharacterEntity playerCharacter = entityManager.CreatePlayerCharacter(prefab, NetworkManager.Instance.ClientController.NetClient.connection.connectionId, 0);
        EntityManager.Instance.AddEntity(playerCharacter.netId.Value, playerCharacter);
        EntityManager.Instance.MyCharacter = playerCharacter;

        mainCamera.followTarget = playerCharacter.gameObject;
        mainCamera.isFollowTarget = true;
    }

    private void SpawnMyCharacter()
    {
        if(!ClientScene.ready)
        {
            Debug.LogError("Client Scene is not ready!");
            return;
        }

        if(EntityManager.Instance.MyCharacter != null)
            EntityManager.Instance.DestroyEntity(EntityManager.Instance.MyCharacter);

        NetworkClient networkClient = networkManager.ClientController.NetClient;
        short playerControllerId = 0;

        ClientScene.AddPlayer(networkClient.connection, playerControllerId);
    }

    private void SpawnNonPlayerCharacter(Vector3 position, Quaternion rotation)
    {
        //uint id = EntityManager.Instance.GenerateNpcId();
        //if(!IdGenerator.IsValid(id))
        //    return;

        CharacterEntity entity = networkManager.RegisterNonPlayerCharacter(position, rotation);
        NetworkServer.Spawn(entity.gameObject);
    }
}
