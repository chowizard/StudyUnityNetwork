using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Character;
using Chowizard.UnityNetwork.Client.Character.Ai;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public sealed class NetworkEventClientToServerCharacterRotateTo : 
        NetworkEventClientToServer<NetworkMessageCharacterRotateTo>
    {
        public override void Receive(NetworkMessage networkMessage)
        {
            if(networkMessage == null)
                return;

            NetworkMessageCharacterRotateTo detailMessage = networkMessage.ReadMessage<NetworkMessageCharacterRotateTo>();
            if(detailMessage == null)
                return;

            CharacterEntity character = EntityManager.Instance.GetEntity(detailMessage.characterId);
            if(character == null)
                return;
            
            CharacterComponentAi aiComponent = character.GetCharacterComponent<CharacterComponentAi>();
            Debug.Assert(aiComponent != null);

            CharacterAiConditionNormal aiCondition = new CharacterAiConditionNormal(character);
            CharacterAiBehaviourMoveToPosition aiBehaviour = new CharacterAiBehaviourMoveToPosition(character, character.transform.position, detailMessage.rotation);

            aiComponent.ChangeAiState(CharacterAiState.eType.Move, aiCondition, aiBehaviour);
        }

        public override short MessageCode
        {
            get
            {
                return NetworkMessageCode.CharacterRotateTo;
            }
        }
    }
}