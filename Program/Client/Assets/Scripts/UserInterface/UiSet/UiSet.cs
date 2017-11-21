using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public abstract class UiSet : MonoBehaviour
    {
        public UiFrame.eAnchorType anchorType = UiFrame.eAnchorType.Center;
    }
}
