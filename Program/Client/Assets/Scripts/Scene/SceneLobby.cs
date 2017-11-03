using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SceneLobby : GameScene
{
    private void Awake()
    {
        sceneType = eSceneType.Lobby;
    }

    // Use this for initialization
    private void Start()
    {
        GameSceneManager.Instance.currentScene = this;

        Debug.Log(string.Format("Scene [{0}] was started.", name));
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
