using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiFrameSceneGamePlay : UiFrame
    {
        [Space]
        public Canvas uiCanvasWorldMain;

        [Space]
        public UiHudCharacterManager uiHudCharacterManager;

        // Use this for initialization
        protected override void Start()
        {
            Debug.Assert(uiCanvasWorldMain != null);

            base.Start();

            LoadUiSets();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        private void LoadUiSets()
        {
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
    }
}
