using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SceneIntro : GameScene
{
    private void Awake()
    {
        sceneType = eSceneType.Intro;
    }

    // Use this for initialization
    private void Start()
    {
        GameSceneManager.Instance.currentScene = this;

        Debug.Log(string.Format("Scene [{0}] was started.", name));

        UiManager.Instance.LoadUiFrameFromFile("UserInterface/UiFrame/ControlPanel");
        UiManager.Instance.LoadUiFrameFromFile("UserInterface/UiFrame/InformationWindow");
        UiManager.Instance.LoadUiFrameFromFile("UserInterface/UiFrame/LogWindow");
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
