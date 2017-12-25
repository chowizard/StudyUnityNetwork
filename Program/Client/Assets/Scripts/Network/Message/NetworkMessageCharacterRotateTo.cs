using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public class NetworkMessageCharacterRotateTo : NetworkCustomMessage
    {
        public uint characterId;
        public Quaternion rotation;

        public override short MessageCode
        {
            get
            {
                return NetworkMessageCode.CharacterRotateTo;
            }
        }

        /* UnityEntgine.Networking.NetworkMessage에서 UnityEntgine.Networking.MessageBase 
          기반의 객체로 전달할 때, public 기본 생성자를 요구한다. */
        public NetworkMessageCharacterRotateTo()
        {
            /* 구현 없음 */
        }

        public NetworkMessageCharacterRotateTo(uint characterId, Quaternion rotation)
        {
            this.characterId = characterId;
            this.rotation = rotation;
        }
    }
}
