using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public abstract class NetworkEventHandler
    {
        public abstract void Receive(NetworkMessage networkMessage);

        public abstract short MessageCode
        {
            get;
        }
    }
}