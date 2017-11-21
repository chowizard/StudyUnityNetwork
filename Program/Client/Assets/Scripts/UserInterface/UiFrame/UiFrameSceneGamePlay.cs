using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiFrameSceneGamePlay : UiFrame
    {

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            switch(NetworkManager.Instance.mode)
            {
            case NetworkManager.eMode.Server:
                LoadUiSet("UiSetControlPanelServer");
                break;

            case NetworkManager.eMode.Client:
                LoadUiSet("UiSetControlPanelClient");
                break;
            }

            LoadUiSet("UiSetInformationWindow");
            LoadUiSet("UiSetLogWindow");
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}
