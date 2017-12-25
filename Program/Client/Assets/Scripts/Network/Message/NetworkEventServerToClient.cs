using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public abstract class NetworkEventServerToClient<ClassType> : 
        NetworkEventHandler 
        where ClassType : NetworkCustomMessage
    {   
        public void SendToClient(ClassType networkMessage, int connectionId)
        {
            if(networkMessage == null)
                return;

            if(connectionId < 0)
                return;

            NetworkServer.SendToClient(connectionId, networkMessage.MessageCode, networkMessage);
        }

        public void SendToClients(ClassType networkMessage, IEnumerable<int> connectionIds)
        {
            if(connectionIds == null)
                return;

            foreach(int connectionId in connectionIds)
                SendToClient(networkMessage, connectionId);
        }
        
        public void SendToAll(ClassType networkMessage)
        {
            if(networkMessage == null)
                return;

            NetworkServer.SendToAll(networkMessage.MessageCode, networkMessage);
        }

        public void SendByChannelToAll(ClassType networkMessage, short channelId)
        {
            if(networkMessage == null)
                return;

            if(channelId < 0)
                return;

            NetworkServer.SendByChannelToAll(networkMessage.MessageCode, networkMessage, channelId);
        }

        public void SendByChannelsToAll(ClassType networkMessage, IEnumerable<short> channelIds)
        {
            if(networkMessage == null)
                return;

            if(channelIds == null)
                return;

            foreach(short channelId in channelIds)
                SendByChannelToAll(networkMessage, channelId);
        }

        public void SendUnreliableToAll(ClassType networkMessage)
        {
            if(networkMessage == null)
                return;

            NetworkServer.SendUnreliableToAll(networkMessage.MessageCode, networkMessage);
        }
    }
}