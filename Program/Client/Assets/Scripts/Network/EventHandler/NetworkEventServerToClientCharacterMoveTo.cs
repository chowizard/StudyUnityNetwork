using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Network;
using Chowizard.UnityNetwork.Client.Network.Message;

namespace Chowizard.UnityNetwork.Client.Network.EventHandler
{
    public sealed class NetworkEventServerToClientCharacterMoveTo : NetworkEventServerToClient
    {
        public override void SendToClientByChannel(int connectionId, short channelId = Channels.DefaultReliable)
        {

        }
        public override void SendToAllByChannel(short channelId = Channels.DefaultReliable)
        {
            
        }

        public override void Receive(NetworkMessage networkMessage)
        {

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