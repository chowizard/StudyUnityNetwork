using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Chowizard.UnityNetwork.Client.Scene;

namespace Chowizard.UnityNetwork.Client.Core
{
    public class GameSceneManager : MonoBehaviour
    {
        public GameScene currentScene;

        public static GameSceneManager Instance
        {
            get
            {
                return GameManager.Singleton.gameSceneManager;
            }
        }

        public void ChangeScene(GameScene.eSceneType sceneType)
        {
            switch(sceneType)
            {
            case GameScene.eSceneType.Intro:
                LoadScene("Intro");
                break;

            case GameScene.eSceneType.Lobby:
                LoadScene("Lobby");
                break;

            case GameScene.eSceneType.GamePlay:
                LoadScene("GamePlay");
                break;
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}
