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
        public void SendByChannel(NetworkClient networkClient, ClassType networkMessage, int channelId)
        {
            if(networkClient == null)
                return;

            if(networkMessage == null)
                return;

            networkClient.SendByChannel(networkMessage.MessageCode, networkMessage, channelId);
        }
        
        public void Send(NetworkClient networkClient, ClassType networkMessage)
        {
            if(networkClient == null)
                return;

            if(networkMessage == null)
                return;

            networkClient.Send(networkMessage.MessageCode, networkMessage);
        }

        public void SendUnreliable(NetworkClient networkClient, ClassType networkMessage)
        {
            if(networkClient == null)
                return;

            if(networkMessage == null)
                return;

            networkClient.SendUnreliable(networkMessage.MessageCode, networkMessage);
        }
    }
}