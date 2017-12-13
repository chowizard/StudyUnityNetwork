using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Network.Message
{
    public class NetworkMessageCharacterMoveTo : MessageBase
    {
        public uint netId;
        public Vector3 position;
    }
}
