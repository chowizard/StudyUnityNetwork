using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;
using Chowizard.UnityNetwork.Client.Ui;

namespace Chowizard.UnityNetwork.Client.Scene
{
    public class SceneLobby : GameScene
    {
        private UiFrame uiFrameLobby;

        private void Awake()
        {
            sceneType = eSceneType.Lobby;
        }

        // Use this for initialization
        private void Start()
        {
            GameSceneManager.Instance.currentScene = this;
            Debug.Log(string.Format("Scene [{0}] was started.", sceneType.ToString()));

            uiFrameLobby = UiManager.Instance.LoadUiFrame("UiFrameLobby");
            UiManager.Instance.ChangeUiFrame(uiFrameLobby);
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}

