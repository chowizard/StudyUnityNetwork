using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiSetVersion : UiSet
    {
        public Text uiTextVersion;
        private uint versionCode;

        // Use this for initialization
        private void Start()
        {
            Debug.Assert(uiTextVersion != null);

            RefreshVersion();
        }

        // Update is called once per frame
        private void Update()
        {
            //RefreshVersion();
        }

        private void RefreshVersion()
        {
            if(versionCode == Version.Code)
                return;

            versionCode = Version.Code;
            uiTextVersion.text = string.Format("Version : {0}", Version.String);
        }
    }
}
