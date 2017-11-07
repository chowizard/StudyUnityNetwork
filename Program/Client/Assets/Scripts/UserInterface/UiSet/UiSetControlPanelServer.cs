﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiSetControlPanelServer : UiSet
{
    public Button buttonTerminate;

    public void OnClickTerminate()
    {
        NetworkManager.Instance.Terminate();
        GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.Intro);
    }

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(buttonTerminate != null);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
