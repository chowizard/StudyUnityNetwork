using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFrameStart : UiFrame
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        LoadUiSet("UiSetInformationWindow");
        LoadUiSet("UiSetLogWindow");
        LoadUiSet("UiSetControlPanelSelectMode");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
