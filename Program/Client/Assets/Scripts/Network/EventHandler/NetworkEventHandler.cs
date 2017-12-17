using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.EventHandler
{
    public abstract class NetworkEventHandler<ClassType> where ClassType : MessageBase
    {
        public abstract void Receive(NetworkMessage networkMessage);

        public abstract short MessageCode
        {
            get;
        }
    }
}