using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using UnityNet.Client.Core;

public class SceneGamePlay : GameScene
{
    private UiFrame uiFrameGamePlay;

    public void SpawnNonPlayerCharacters(int spawningCount)
    {
        for(int count = 0; count < spawningCount; ++count)
        {
            float positionX = Random.Range(-100.0f, 100.0f);
            float positionZ = Random.Range(-100.0f, 100.0f);
            Vector3 position = new Vector3(positionX, 0.0f, positionZ);

            float rotationY = Random.Range(0.0f, 360.0f);
            Quaternion rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);

            SpawnNonPlayerCharacter(position, rotation);
        }
    }

    public void ClearEntities()
    {
        EntityManager.Instance.Clear();
    }

    private void Awake()
    {
        sceneType = eSceneType.GamePlay;
    }

    // Use this for initialization
    private void Start()
    {
        GameSceneManager.Instance.currentScene = this;
        Debug.Log(string.Format("Scene [{0}] was started.", sceneType.ToString()));

        uiFrameGamePlay = UiManager.Instance.LoadUiFrame("UiFrameGamePlay");
        UiManager.Instance.ChangeUiFrame(uiFrameGamePlay);

        switch(NetworkManager.Instance.mode)
        {
        case NetworkManager.eMode.Server:
            {
                SpawnNonPlayerCharacters(GameManager.Singleton.initialNpcSpawningCount);
            }
            break;

        case NetworkManager.eMode.Client:
            {
                SpawnMyCharacter();
            }
            break;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDestroy()
    {
        ClearEntities();
    }

    private void OnApplicationQuit()
    {
        ClearEntities();
    }

    private void GenerateMyCharacter()
    {
        GameObject prefab = NetworkManager.Instance.GetSpawningPrefab(Defines.SpawningPrefab.PlayerCharacter);

        CharacterEntity playerCharacter = EntityManager.Instance.CreatePlayerCharacter(prefab, NetworkManager.Instance.ClientController.NetClient.connection.connectionId, 0);
        EntityManager.Instance.AddEntity(playerCharacter.netId.Value, playerCharacter);
        EntityManager.Instance.MyCharacter = playerCharacter;

        GameManager.Singleton.mainCamera.followTarget = playerCharacter.gameObject;
        GameManager.Singleton.mainCamera.isFollowTarget = true;
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

        NetworkClient networkClient = NetworkManager.Instance.ClientController.NetClient;
        short playerControllerId = 0;

        ClientScene.AddPlayer(networkClient.connection, playerControllerId);
    }

    private void SpawnNonPlayerCharacter(Vector3 position, Quaternion rotation)
    {
        CharacterEntity entity = NetworkManager.Instance.RegisterNonPlayerCharacter(position, rotation);
        NetworkServer.Spawn(entity.gameObject);
    }
}
