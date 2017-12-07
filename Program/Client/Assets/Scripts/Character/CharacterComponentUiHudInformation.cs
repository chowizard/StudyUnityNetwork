using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Ui;

namespace Chowizard.UnityNetwork.Client.Character
{
    [DisallowMultipleComponent]
    public class CharacterComponentUiHudInformation : CharacterComponent
    {
        public UiHudCharacterInformation uiHud;

        // Use this for initialization
        protected override void Start()
        {
            if(uiHud == null)
            {
                UiFrameSceneGamePlay uiFrame = UiManager.Instance.GetUiFrame("UiFrameSceneGamePlay") as UiFrameSceneGamePlay;
                Debug.Assert(uiFrame != null);

                if(uiFrame != null)
                {
                    uiHud = uiFrame.uiHudCharacterManager.Get(owner.netId.Value);
                    if(uiHud == null)
                    {
                        //uiHud = Instantiate<Ui>
                        //uiFrame.uiHudCharacterManager.Add(owner, );
                    }
                }
            }

            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        private void OnDestroy()
        {

        }
    }

}
