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

        [Space]
        public Transform pointCharacterId;
        public Transform pointDescription;

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
                uiCharacterInformation = uiFrame.uiHudCharacterManager.Register(this);

            return uiCharacterInformation;
        }
    }
}
