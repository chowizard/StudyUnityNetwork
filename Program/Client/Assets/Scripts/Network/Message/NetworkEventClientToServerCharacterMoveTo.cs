using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Character;
using Chowizard.UnityNetwork.Client.Character.Ai;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public sealed class NetworkEventClientToServerCharacterMoveTo : 
        NetworkEventClientToServer<NetworkMessageCharacterMoveTo>
    {
        public override void Receive(NetworkMessage networkMessage)
        {
            if(networkMessage == null)
                return;

            NetworkMessageCharacterMoveTo detailMessage = networkMessage.ReadMessage<NetworkMessageCharacterMoveTo>();
            if(detailMessage == null)
                return;

            CharacterEntity character = EntityManager.Instance.GetEntity(detailMessage.characterId);
            if(character == null)
                return;
            
            CharacterComponentAi aiComponent = character.GetCharacterComponent<CharacterComponentAi>();
            Debug.Assert(aiComponent != null);
            Vector3 toDirection = Vector3.Normalize(detailMessage.position - character.transform.position);
            if(toDirection == Vector3.zero)
                return;
            
            Quaternion toRotation = Quaternion.FromToRotation(character.transform.forward, toDirection);
            Quaternion nowRotation = character.transform.rotation * toRotation;

            CharacterAiConditionNormal aiCondition = new CharacterAiConditionNormal(character);
            CharacterAiBehaviourMoveToPosition aiBehaviour = new CharacterAiBehaviourMoveToPosition(character, detailMessage.position, nowRotation);

            aiComponent.ChangeAiState(CharacterAiState.eType.Move, aiCondition, aiBehaviour);
        }

        public override short MessageCode
        {
            get
            {
                return NetworkMessageCode.CharacterMoveTo;
            }
        }
    }
}