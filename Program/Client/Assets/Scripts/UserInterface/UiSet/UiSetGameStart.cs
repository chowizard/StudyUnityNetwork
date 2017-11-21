using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiSetGameStart : UiSet
    {
        public Button buttonGameStart;

        public void OnClickGameStart()
        {
            if(NetworkManager.Instance.mode != NetworkManager.eMode.Client)
                return;

            NetworkManager.Instance.StartByClient();

            //if(NetworkManager.Instance.isAtStartup == false)
            //    GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);
            //else
            //    Debug.LogError("NetworkManager is not startup.");
        }

        // Use this for initialization
        private void Start()
        {
            Debug.Assert(buttonGameStart != null);
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}

