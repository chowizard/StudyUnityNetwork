using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UiSetControlPanelGameStart : UiSet
{
    public Button uiButtonGoToBack;

    public void OnClickGoToBack()
    {
        GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.Intro);
    }

    // Use this for initialization
    private void Start()
    {
        Debug.Assert(uiButtonGoToBack != null);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
