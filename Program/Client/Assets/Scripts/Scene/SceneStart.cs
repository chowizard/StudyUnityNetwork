using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;
using Chowizard.UnityNetwork.Client.Ui;

namespace Chowizard.UnityNetwork.Client.Scene
{
    public class SceneStart : GameScene
    {
        private UiFrame uiFrameStart;

        private void Awake()
        {
            sceneType = eSceneType.Start;
        }

        // Use this for initialization
        private void Start()
        {
            GameSceneManager.Instance.currentScene = this;
            Debug.Log(string.Format("Scene [{0}] was started.", sceneType.ToString()));

            uiFrameStart = UiManager.Instance.LoadUiFrame("UiFrameSceneStart");
            UiManager.Instance.ChangeUiFrame(uiFrameStart);
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}

