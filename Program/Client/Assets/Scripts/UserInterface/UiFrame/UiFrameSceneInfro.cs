using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiFrameSceneInfro : UiFrame
    {

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            LoadUiSet("UiSetControlPanelSelectMode");
            LoadUiSet("UiSetInformationWindow");
            LoadUiSet("UiSetLogWindow");
            LoadUiSet("UiSetVersion");
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}
