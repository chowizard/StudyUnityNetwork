﻿using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiConditionSenseEnemy : CharacterAiCondition
    {
        public CharacterAiConditionSenseEnemy(CharacterEntity owner) :
            base(eType.SensedEnemy, owner)
        {
        }
    }
}