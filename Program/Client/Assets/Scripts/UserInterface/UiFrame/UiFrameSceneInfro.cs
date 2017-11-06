using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFrameSceneInfro : UiFrame
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        LoadUiSetFromFile("UserInterface/UiSet/Intro/UiSetControlPanel");
        LoadUiSetFromFile("UserInterface/UiSet/Intro/UiSetInformationWindow");
        LoadUiSetFromFile("UserInterface/UiSet/Intro/UiSetLogWindow");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
