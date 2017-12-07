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
        public CharacterEntity owner;

        [Space]
        public Text uiTextCharacterId;
        public Text uiTextDescription;

        private uint characterId;
        private Vector3 characterPosition;

        // Use this for initialization
        private void Start()
        {
            Debug.Assert(owner != null);

            Debug.Assert(uiTextCharacterId != null);
            Debug.Assert(uiTextDescription != null);
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateCheckCharacterId();
            UpdateCheckDescription();
        }

        private void UpdateCheckCharacterId()
        {
            if(characterId == owner.netId.Value)
                return;

            characterId = owner.netId.Value;
            uiTextCharacterId.text = DisplayTextCharacterId;
        }

        private void UpdateCheckDescription()
        {

        }

        private string DisplayTextCharacterId
        {
            get
            {
                return "ID : ";
            }
        }

        private string DisplayTextDescription
        {
            get
            {
                string stateText = "상태 : ";
                string positionText = "위치 : " + owner.transform.position;

                return stateText + "\n" + positionText;
            }
        }
    }
}
