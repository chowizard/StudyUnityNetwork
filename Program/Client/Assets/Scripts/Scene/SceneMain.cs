﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class SceneMain : MonoBehaviour
{
    public string sceneName;
    public EntityManager entityManager;
    public NetworkManager networkManager;

    private static SceneMain singleton;

    public SceneMain Singleton
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
        //if(networkManager.mode == NetworkManager.Mode.Server)
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
        //networkManager.AddPlayer(playerControllerId);
        ClientScene.AddPlayer(networkClient.connection, playerControllerId);
    }
}