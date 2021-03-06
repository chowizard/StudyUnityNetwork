﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character.Action
{
    public class CharacterAction
    {
        public enum eType
        {
            None,

            Idle,
            Die,
            Move,
            Attack,
            UseSkill
        }

        private eType type = eType.None;

        public CharacterAction(eType type)
        {
            this.type = type;
        }

        public eType Type
        {
            get
            {
                return type;
            }
        }
    }
}
