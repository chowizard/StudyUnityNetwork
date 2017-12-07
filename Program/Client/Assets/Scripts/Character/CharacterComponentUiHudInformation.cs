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
            base.Start();

            if(uiHud == null)
                uiHud = RegisterUi();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        private void OnDestroy()
        {

        }

        private UiHudCharacterInformation RegisterUi()
        {
            UiFrameSceneGamePlay uiFrame = UiManager.Instance.GetUiFrame("UiFrameSceneGamePlay") as UiFrameSceneGamePlay;
            Debug.Assert(uiFrame != null);

            if(uiFrame == null)
                return null;

            UiHudCharacterInformation uiCharacterInformation = uiFrame.uiHudCharacterManager.Get(owner.netId.Value);
            if(uiCharacterInformation == null)
            {
                GameObject uiHudObject = ResourceManager.Instance.InstantiateFromResource("UserInterface/UiHeadupDisplay/UiHudCharacterInformation");
                uiCharacterInformation = uiHudObject.GetComponent<UiHudCharacterInformation>();
                Debug.Assert(uiCharacterInformation != null);

                uiFrame.uiHudCharacterManager.Add(owner, uiCharacterInformation);
            }

            return uiCharacterInformation;
        }
    }
}
