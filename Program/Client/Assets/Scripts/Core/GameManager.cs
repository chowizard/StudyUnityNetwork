﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using UnityNet.Client.Core;

public class GameManager : MonoBehaviour
{
    public int npcCount = 100;

    public CameraController mainCamera;

    private EntityManager entityManager;
    private NetworkManager networkManager;
    private UiManager uiManager;

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

    public EntityManager EntityManager
    {
        get
        {
            return entityManager;
        }
    }

    public NetworkManager NetworkManager
    {
        get
        {
            return networkManager;
        }
    }

    public UiManager UiManager
    {
        get
        {
            return uiManager;
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
        if(entityManager == null)
        {
            Transform findTransform = transform.Find("EntityManager");
            if(findTransform != null)
                entityManager = findTransform.GetComponent<EntityManager>();
        }

        if(networkManager == null)
        {
            Transform findTransform = transform.Find("NetworkManager");
            if(findTransform != null)
                networkManager = findTransform.GetComponent<NetworkManager>();
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

        CharacterEntity playerCharacter = EntityManager.CreatePlayerCharacter(prefab, NetworkManager.Instance.ClientController.NetClient.connection.connectionId, 0);
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