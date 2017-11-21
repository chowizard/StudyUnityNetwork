using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;
using Chowizard.UnityNetwork.Client.Scene;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiSetControlPanelSelectMode : UiSet
    {
        public Button uiButtonStartByServer;
        public Button uiButtonStartByClient;

        public void OnClickStartByServer()
        {
            NetworkManager.Instance.StartByServer();

            if(NetworkManager.Instance.isAtStartup == false)
                GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);
            else
                Debug.LogError("NetworkManager is not startup.");
        }

        public void OnClickStartByClient()
        {
            NetworkManager.Instance.mode = NetworkManager.eMode.Client;
            GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.Lobby);
        }

        // Use this for initialization
        private void Start()
        {
            Debug.Assert(uiButtonStartByServer != null);
            Debug.Assert(uiButtonStartByClient != null);
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}
