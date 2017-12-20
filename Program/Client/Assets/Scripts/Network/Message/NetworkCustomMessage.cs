using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public abstract class NetworkCustomMessage : MessageBase
    {
        public abstract short MessageCode
        {
            get;
        }
    }
}