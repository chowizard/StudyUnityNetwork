using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiBehaviourReturnTo : CharacterAiBehaviour
    {
        public CharacterAiBehaviourReturnTo(CharacterEntity owner) :
            base(eType.ReturnTo, owner)
        {
        }
    }
}
