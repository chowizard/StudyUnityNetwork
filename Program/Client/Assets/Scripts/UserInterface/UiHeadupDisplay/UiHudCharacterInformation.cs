using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Character;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiHudCharacterInformation : MonoBehaviour
    {
        public CharacterComponentUiHudInformation controller;

        [Space]
        public Text uiTextCharacterId;
        public Text uiTextDescription;

        private uint characterId;
        private Vector3 characterPosition;

        // Use this for initialization
        private void Start()
        {
            Debug.Assert(controller != null);

            Debug.Assert(uiTextCharacterId != null);
            Debug.Assert(uiTextDescription != null);
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateCheckCharacterId();
            UpdateCheckDescription();
        }

        private void LateUpdate()
        {
            if(controller == null)
                return;

            transform.position = controller.transform.position;

            uiTextCharacterId.transform.position = controller.pointCharacterId.transform.position;
            uiTextDescription.transform.position = controller.pointDescription.transform.position;
        }

        private void UpdateCheckCharacterId()
        {
            if(controller == null)
                return;

            Debug.Assert(controller.owner != null);
            if(controller.owner == null)
                return;

            uint id = controller.owner.netId.Value;

            if(characterId == id)
                return;

            characterId = id;
            uiTextCharacterId.text = DisplayTextCharacterId;
        }

        private void UpdateCheckDescription()
        {
            if(controller == null)
                return;

            if(characterPosition == controller.transform.position)
                return;

            characterPosition = controller.transform.position;
            uiTextDescription.text = DisplayTextDescription;
        }

        private string DisplayTextCharacterId
        {
            get
            {
                return "ID : " + characterId;
            }
        }

        private string DisplayTextDescription
        {
            get
            {
                string stateText = "상태 : ";
                string positionText = "위치 : " + characterPosition;

                return stateText + "\n" + positionText;
            }
        }
    }
}
