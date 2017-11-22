using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiBehaviourChase : CharacterAiBehaviour
    {
        public CharacterAiBehaviourChase(CharacterEntity owner) :
            base(eType.Chase, owner)
        {
        }
    }
}
