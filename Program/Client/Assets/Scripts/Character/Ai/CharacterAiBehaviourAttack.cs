﻿using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiBehaviourAttack : CharacterAiBehaviour
    {
        public CharacterAiBehaviourAttack(CharacterEntity owner) :
            base(eType.Attack, owner)
        {
        }
    }
}
