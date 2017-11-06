using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFrameSceneGamePlay : UiFrame
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        switch(NetworkManager.Instance.mode)
        {
        case NetworkManager.eMode.Server:
            UiManager.Instance.LoadUiFrame("UiSetControlPanelServer");
            break;

        case NetworkManager.eMode.Client:
            UiManager.Instance.LoadUiFrame("UiSetControlPanelClient");
            break;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
