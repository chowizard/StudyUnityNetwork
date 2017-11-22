using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiConditionDead : CharacterAiCondition
    {
        public CharacterAiConditionDead(CharacterEntity owner) :
            base(eType.Dead, owner)
        {
        }
    }
}
