using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiFrameSceneLobby : UiFrame
    {
        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            LoadUiSet("UiSetInformationWindow");
            LoadUiSet("UiSetLogWindow");
            LoadUiSet("UiSetGameStart");
            LoadUiSet("UiSetControlPanelGameStart");
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}
