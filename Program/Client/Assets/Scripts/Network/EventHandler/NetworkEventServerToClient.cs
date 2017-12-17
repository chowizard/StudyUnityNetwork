using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.EventHandler
{
    public abstract class NetworkEventServerToClient : NetworkEventHandler
    {
        public abstract void SendToClientByChannel(int connectionId, short channelId = Channels.DefaultReliable);
        public abstract void SendToAllByChannel(short channelId = Channels.DefaultReliable);
        
        public void SendToClient(int connectionId)
        {
            SendToClientByChannel(connectionId, Channels.DefaultReliable);
        }
        
        public void SendToAll()
        {
            SendToAllByChannel(Channels.DefaultReliable);
        }

        public void SendUnreliableToClient(int connectionId)
        {
            SendToClientByChannel(connectionId, Channels.DefaultUnreliable);
        }
        
        public void SendUnreliableToAll()
        {
            SendToClientByChannel(Channels.DefaultUnreliable);
        }
    }
}