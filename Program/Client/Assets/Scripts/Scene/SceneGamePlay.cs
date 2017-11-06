using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SceneGamePlay : GameScene
{
    private void Awake()
    {
        sceneType = eSceneType.GamePlay;
    }

    // Use this for initialization
    private void Start()
    {
        GameSceneManager.Instance.currentScene = this;
        Debug.Log(string.Format("Scene [{0}] was started.", sceneType.ToString()));

        UiManager.Instance.LoaUiFrame("UiFrameGamePlay");
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
