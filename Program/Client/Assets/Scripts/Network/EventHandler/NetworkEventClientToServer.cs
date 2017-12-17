using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.EventHandler
{
    public abstract class NetworkEventClientToServer : NetworkEventHandler
    {
        public abstract void SendByChannel(int channelId = Channels.DefaultReliable);
        
        public void Send()
        {
            SendByChannel(Channels.DefaultReliable);
        }

        public void SendUnreliable()
        {
            SendByChannel(Channels.DefaultUnreliable);
        }
    }
}