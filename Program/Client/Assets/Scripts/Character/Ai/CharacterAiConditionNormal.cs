using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiConditionNormal : CharacterAiCondition
    {
        public CharacterAiConditionNormal(CharacterEntity owner) :
            base(eType.Normal, owner)
        {
        }
    }
}
