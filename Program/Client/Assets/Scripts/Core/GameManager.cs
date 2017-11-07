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
                    GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);
                else
                    Debug.LogError("NetworkManager is not startup.");
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
    }
}
