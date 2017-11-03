using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFrameSceneInfro : UiFrame
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        LoadUiSetFromFile("UserInterface/UiSet/ControlPanel");
        LoadUiSetFromFile("UserInterface/UiSet/InformationWindow");
        LoadUiSetFromFile("UserInterface/UiSet/LogWindow");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
