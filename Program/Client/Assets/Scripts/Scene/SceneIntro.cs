﻿using System.Collections;
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
        Debug.Log(string.Format("Scene [{0}] was started.", name));
    }

    // Update is called once per frame
    private void Update()
    {

    }
}