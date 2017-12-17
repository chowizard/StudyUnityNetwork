using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public sealed class NetworkEventClientToServerCharacterMoveTo : 
        NetworkEventClientToServer<NetworkMessageCharacterMoveTo>
    {
        public override void SendByChannel(NetworkMessageCharacterMoveTo networkMessage, int channelId)
        {
            if(networkMessage == null)
                return;
        }

        public override void Receive(NetworkMessage networkMessage)
        {
            if(networkMessage == null)
                return;
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