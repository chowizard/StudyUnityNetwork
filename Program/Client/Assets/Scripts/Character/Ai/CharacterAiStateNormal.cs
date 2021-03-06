﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiStateNormal : CharacterAiState
    {
        public CharacterAiStateNormal(CharacterComponentAi stateManager) :
            base(stateManager)
        {
            type = eType.Normal;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
