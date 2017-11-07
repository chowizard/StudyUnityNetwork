using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SceneGamePlay : GameScene
{
    private UiFrame uiFrameGamePlay;

    private void Awake()
    {
        sceneType = eSceneType.GamePlay;
    }

    // Use this for initialization
    private void Start()
    {
        GameSceneManager.Instance.currentScene = this;
        Debug.Log(string.Format("Scene [{0}] was started.", sceneType.ToString()));

        uiFrameGamePlay = UiManager.Instance.LoadUiFrame("UiFrameGamePlay");
        UiManager.Instance.ChangeUiFrame(uiFrameGamePlay);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
