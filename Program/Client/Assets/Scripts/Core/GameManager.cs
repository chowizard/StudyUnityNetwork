using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Scene;
using Chowizard.UnityNetwork.Client.Ui;

namespace Chowizard.UnityNetwork.Client.Core
{
    public class GameManager : MonoBehaviour
    {
        public Network.NetworkManager.eMode modeStartAt = Network.NetworkManager.eMode.None;
        public int initialNpcSpawningCount = 100;

        public CameraController mainCamera;

        public GameSceneManager gameSceneManager;
        public GameOptionManager gameOptionManager;
        public ResourceManager resourceManager;
        public EntityManager entityManager;
        public Network.NetworkManager networkManager;
        public UiManager uiManager;

        private static GameManager singleton;

        public static GameManager Singleton
        {
            get
            {
                if(singleton == null)
                    singleton = FindObjectOfType<GameManager>();

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
            case Network.NetworkManager.eMode.Server:
                {
                    Network.NetworkManager.Instance.mode = modeStartAt;
                    Network.NetworkManager.Instance.StartByServer();

                    if(Network.NetworkManager.Instance.isAtStartup == false)
                        GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);
                    else
                        Debug.LogError("NetworkManager is not startup.");
                }
                break;

            case Network.NetworkManager.eMode.Client:
                {
                    Network.NetworkManager.Instance.mode = modeStartAt;
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
}
