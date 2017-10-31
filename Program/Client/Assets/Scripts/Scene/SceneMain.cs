﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using UnityNet.Client.Core;

public class SceneMain : MonoBehaviour
{
    public int npcCount = 100;

    public string sceneName;
    public EntityManager entityManager;
    public NetworkManager networkManager;

    private static SceneMain singleton;

    public static SceneMain Singleton
    {
        get
        {
            return singleton;
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
        Debug.Log(string.Format("Scene [{0}] was started.", sceneName));

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
        if(networkManager.mode == NetworkManager.Mode.Server)
        {
            if(Input.GetKeyDown(KeyCode.X))
                SpawnNonPlayerCharacters();
        }

        if(networkManager.mode == NetworkManager.Mode.Client)
        {
            if(Input.GetKeyDown(KeyCode.Z))
                SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        if(!ClientScene.ready)
        {
            Debug.LogError("Client Scene is not ready!");
            return;
        }

        NetworkClient networkClient = networkManager.ClientController.NetClient;
        short playerControllerId = 0;

        ClientScene.AddPlayer(networkClient.connection, playerControllerId);
    }

    private void SpawnNonPlayerCharacters()
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

    private void SpawnNonPlayerCharacter(Vector3 position, Quaternion rotation)
    {
        int id = EntityManager.Instance.GenerateNpcId();
        if(!IdGenerator.IsValid(id))
            return;

        CharacterEntity entity = networkManager.RegisterNonPlayerCharacter(id, position, rotation);
        NetworkServer.Spawn(entity.gameObject);
    }
}
