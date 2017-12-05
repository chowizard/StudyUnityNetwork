using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;
using Chowizard.UnityNetwork.Client.Ui;

namespace Chowizard.UnityNetwork.Client.Scene
{
    public class SceneIntro : GameScene
    {
        private UiFrame uiFrameIntro;

        private void Awake()
        {
            sceneType = eSceneType.Intro;
        }

        // Use this for initialization
        private void Start()
        {
            GameSceneManager.Instance.currentScene = this;
            Debug.Log(string.Format("Scene [{0}] was started.", sceneType.ToString()));

            NetworkManager.Instance.mode = NetworkManager.eMode.None;

            uiFrameIntro = UiManager.Instance.LoadUiFrame("UiFrameSceneIntro");
            UiManager.Instance.ChangeUiFrame(uiFrameIntro);
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}

