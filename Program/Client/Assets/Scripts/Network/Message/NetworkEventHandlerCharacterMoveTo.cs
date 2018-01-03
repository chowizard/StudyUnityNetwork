using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Character;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public sealed class NetworkEventHandlerCharacterMoveTo : NetworkEventHandler
    {
        public override void Receive(NetworkMessage networkMessage)
        {
            if(networkMessage == null)
                return;

            if(networkMessage.reader.Length <= 0)
            {
                Debug.LogError("Received message bytes are nothing!");
                return;
            }

            NetworkMessageCharacterMoveTo detailMessage = networkMessage.ReadMessage<NetworkMessageCharacterMoveTo>();
            if(detailMessage == null)
                return;

            CharacterEntity character = EntityManager.Instance.GetEntity(detailMessage.characterId);
            if(character == null)
                return;

            CharacterComponentMove moveComponent = character.GetCharacterComponent<CharacterComponentMove>();
            Debug.Assert(moveComponent != null);
            if(moveComponent == null)
                return;

            moveComponent.MoveToPosition(detailMessage.position);
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
