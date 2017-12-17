using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.EventHandler
{
    public abstract class NetworkEventServerToClient<ClassType> : 
        NetworkEventHandler<ClassType> where ClassType : MessageBase
    {
        public abstract void SendToClientByChannel(ClassType networkMessage, int connectionId, short channelId);
        public abstract void SendToAllByChannel(ClassType networkMessage, short channelId = Channels.DefaultReliable);
        
        public void SendToClient(ClassType networkMessage, int connectionId)
        {
            SendToClientByChannel(networkMessage, connectionId, Channels.DefaultReliable);
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
            SendToAllByChannel(networkMessage, Channels.DefaultReliable);
        }

        public void SendToAllByChannels(ClassType networkMessage, IEnumerable<short> channelIds)
        {
            if(channelIds == null)
                return;

            foreach(short channelId in channelIds)
                SendToAllByChannel(networkMessage, channelId);
        }

        public void SendToClientsByChannels(ClassType networkMessage, IEnumerable<int> connectionIds, IEnumerable<short> channelIds)
        {
            if(connectionIds == null)
                return;

            if(channelIds == null)
                return;

            foreach(short channelId in channelIds)
            {
                foreach(int connectionId in connectionIds)
                    SendToClientByChannel(networkMessage, connectionId, channelId);
            }
        }

        public void SendUnreliableToClient(ClassType networkMessage, int connectionId)
        {
            SendToClientByChannel(networkMessage, connectionId, Channels.DefaultUnreliable);
        }

        public void SendUnreliableToClients(ClassType networkMessage, IEnumerable<int> connectionIds)
        {
            if(connectionIds == null)
                return;

            foreach(int connectionId in connectionIds)
                SendUnreliableToClient(networkMessage, connectionId);
        }
        
        public void SendUnreliableToAll(ClassType networkMessage)
        {
            SendToAllByChannel(networkMessage, Channels.DefaultUnreliable);
        }

        public void SendUnreliableToAllByChannels(ClassType networkMessage, IEnumerable<short> channelIds)
        {
            if(channelIds == null)
                return;

            foreach(short channelId in channelIds)
                SendToAllByChannel(networkMessage, channelId);
        }

        public void SendUnreliableToClientsByChannels(ClassType networkMessage, IEnumerable<int> connectionIds, IEnumerable<short> channelIds)
        {
            if(connectionIds == null)
                return;

            if(channelIds == null)
                return;

            foreach(short channelId in channelIds)
            {
                foreach(int connectionId in connectionIds)
                    SendToClientByChannel(networkMessage, connectionId, channelId);
            }
        }
    }
}