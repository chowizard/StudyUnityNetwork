using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public class NetworkMessageCharacterMoveTo : NetworkCustomMessage
    {
        public uint characterId;
        public Vector3 position;

        public override short MessageCode
        {
            get
            {
                return NetworkMessageCode.CharacterMoveTo;
            }
        }

        /* UnityEngine.Networking.NetworkMessage에서 UnityEngine.Networking.MessageBase
          기반의 객체로 전달할 때, public 기본 생성자를 요구한다. */
        public NetworkMessageCharacterMoveTo()
        {
            /* 구현 없음 */
        }

        public NetworkMessageCharacterMoveTo(uint characterId, Vector3 position)
        {
            this.characterId = characterId;
            this.position = position;
        }
    }
}
