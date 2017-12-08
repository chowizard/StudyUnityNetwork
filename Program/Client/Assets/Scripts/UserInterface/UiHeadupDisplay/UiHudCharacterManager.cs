using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Character;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiHudCharacterManager : MonoBehaviour
    {
        private Dictionary<uint, UiHudCharacterInformation> characterInformations = new Dictionary<uint, UiHudCharacterInformation>();

        public void Clear()
        {
            for(int count = 0; count < transform.childCount; ++count)
            {
                Transform child = transform.GetChild(count);
                Debug.Assert(child != null);
                if(child == null)
                    continue;

                Destroy(child.gameObject);
            }

            characterInformations.Clear();
        }

        public UiHudCharacterInformation Register(CharacterComponentUiHudInformation controller)
        {
            if(controller == null)
                return null;

            UiHudCharacterInformation uiHud = Generate(controller);
            Debug.Assert(uiHud != null);
            if(uiHud == null)
                return null;

            Add(uiHud);

            uiHud.transform.parent = transform;

            return uiHud;
        }

        public void Add(UiHudCharacterInformation uiHud)
        {
            Debug.Assert(uiHud != null);
            if(uiHud == null)
                return;

            Debug.Assert(uiHud.controller != null);
            if(uiHud.controller == null)
                return;

            Debug.Assert(uiHud.controller.owner != null);
            if(uiHud.controller.owner == null)
                return;

            uint characterId = uiHud.controller.owner.netId.Value;

            Debug.Assert(characterInformations.ContainsKey(characterId) == false);
            characterInformations.Add(characterId, uiHud);
        }

        public bool Remove(uint characterId)
        {
            UiHudCharacterInformation uiHud = Get(characterId);
            if(uiHud == null)
                return false;

            Destroy(uiHud.gameObject);

            return characterInformations.Remove(characterId);
        }

        public bool Remove(UiHudCharacterInformation uiHud)
        {
            Debug.Assert(uiHud != null);
            if(uiHud == null)
                return false;

            Debug.Assert(uiHud.controller != null);
            if(uiHud.controller == null)
                return Remove(characterInformations.FirstOrDefault(target => (target.Value == uiHud)).Key);
            else
                return Remove(uiHud.controller.owner.netId.Value);

        }

        public UiHudCharacterInformation Get(uint characterId)
        {
            UiHudCharacterInformation data;
            return characterInformations.TryGetValue(characterId, out data) ? data : null;
        }

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        private UiHudCharacterInformation Generate(CharacterComponentUiHudInformation controller)
        {
            if(controller == null)
                return null;

            Debug.Assert(controller.owner != null);
            if(controller.owner == null)
                return null;

            uint characterId = controller.owner.netId.Value;

            UiHudCharacterInformation uiHud = Get(characterId);
            if(uiHud != null)
                return uiHud;

            GameObject uiHudObject = ResourceManager.Instance.InstantiateFromResource("UserInterface/UiHeadupDisplay/UiHudCharacterInformation");
            uiHud = uiHudObject.GetComponent<UiHudCharacterInformation>();
            Debug.Assert(uiHud != null);

            uiHud.controller = controller;
            Debug.Assert(uiHud.controller != null);
            if(uiHud.controller == null)
            {
                Destroy(uiHud.gameObject);
                return null;
            }

            return uiHud;
        }
    }

}
