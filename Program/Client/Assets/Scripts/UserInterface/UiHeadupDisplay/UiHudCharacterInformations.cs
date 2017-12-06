using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Scene;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiHudCharacterInformations : MonoBehaviour
    {
        public Text uiTextCharacterId;
        public Text uiTextDescription;

        private uint characterId;
        private Vector3 characterPosition;

        // Use this for initialization
        private void Start()
        {
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
        }

        private void UpdateCheckDescription()
        {
        }
    }
}
