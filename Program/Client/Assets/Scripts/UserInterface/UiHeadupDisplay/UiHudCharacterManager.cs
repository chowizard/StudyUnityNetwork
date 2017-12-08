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

        public void Add(CharacterEntity owner, UiHudCharacterInformation uiHud)
        {
            Debug.Assert(owner != null);
            if(owner == null)
                return;

            Debug.Assert(uiHud != null);
            if(uiHud == null)
                return;

            if(uiHud.owner != owner)
                uiHud.owner = owner;

            Debug.Assert(characterInformations.ContainsKey(owner.netId.Value) == false);
            characterInformations.Add(owner.netId.Value, uiHud);
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

            Debug.Assert(uiHud.owner != null);
            if(uiHud.owner == null)
                return Remove(characterInformations.FirstOrDefault(target => (target.Value == uiHud)).Key);
            else
                return Remove(uiHud.owner.netId.Value);

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
    }

}
