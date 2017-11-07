using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiSetGameStart : UiSet
{
    public Button buttonGameStart;

    public void OnClickGameStart()
    {
        if(NetworkManager.Instance.mode != NetworkManager.eMode.Client)
            return;

        NetworkManager.Instance.StartByClient();
        GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);
    }

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(buttonGameStart != null);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
