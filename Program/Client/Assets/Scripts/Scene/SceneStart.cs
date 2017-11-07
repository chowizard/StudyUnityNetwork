using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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

        uiFrameStart = UiManager.Instance.LoadUiFrame("UiFrameStart");
        UiManager.Instance.ChangeUiFrame(uiFrameStart);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
