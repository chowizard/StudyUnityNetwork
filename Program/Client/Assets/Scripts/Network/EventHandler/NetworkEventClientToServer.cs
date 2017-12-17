using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.EventHandler
{
    public abstract class NetworkEventClientToServer<ClassType> : 
        NetworkEventHandler<ClassType> where ClassType : MessageBase
    {
        public abstract void SendByChannel(ClassType networkMessage, int channelId);
        
        public void Send(ClassType networkMessage)
        {
            SendByChannel(networkMessage, Channels.DefaultReliable);
        }

        public void SendUnreliable(ClassType networkMessage)
        {
            SendByChannel(networkMessage, Channels.DefaultUnreliable);
        }
    }
}