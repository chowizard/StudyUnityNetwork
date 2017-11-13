﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiSetControlPanelClient : UiSet
{
    public Button buttonTerminate;

    public void OnClickTerminate()
    {
        NetworkManager.Instance.Terminate();
        GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.Lobby);
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