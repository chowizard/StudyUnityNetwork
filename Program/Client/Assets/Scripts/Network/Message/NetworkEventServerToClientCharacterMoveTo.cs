using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public sealed class NetworkEventServerToClientCharacterMoveTo : 
        NetworkEventServerToClient<NetworkMessageCharacterMoveTo>
    {
        public override void SendToClientByChannel(NetworkMessageCharacterMoveTo networkMessage, int connectionId, short channelId)
        {
            if(networkMessage == null)
                return;
        }
        public override void SendToAllByChannel(NetworkMessageCharacterMoveTo networkMessage, short channelId)
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