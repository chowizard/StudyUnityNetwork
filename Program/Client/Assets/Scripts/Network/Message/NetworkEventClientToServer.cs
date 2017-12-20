using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public abstract class NetworkEventClientToServer<ClassType> : 
        NetworkEventHandler 
        where ClassType : NetworkCustomMessage
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