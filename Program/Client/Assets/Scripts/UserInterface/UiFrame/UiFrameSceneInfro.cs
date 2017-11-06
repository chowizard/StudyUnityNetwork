using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFrameSceneInfro : UiFrame
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        LoadUiSetFromFile("UserInterface/UiSet/UiSetControlPanelSelectMode");
        LoadUiSetFromFile("UserInterface/UiSet/UiSetInformationWindow");
        LoadUiSetFromFile("UserInterface/UiSet/UiSetLogWindow");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
