using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Network;
using Chowizard.UnityNetwork.Client.Network.Message;

namespace Chowizard.UnityNetwork.Client.Network.EventHandler
{
    public sealed class NetworkEventClientToServerCharacterMoveTo : NetworkEventClientToServer
    {
        public override void SendByChannel(int channelId = Channels.DefaultReliable)
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