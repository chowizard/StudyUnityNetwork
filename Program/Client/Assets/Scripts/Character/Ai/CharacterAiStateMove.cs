using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SnowFamily.UnityNet.Client.Ai
{
    public class CharacterAiStateMove : CharacterAiState
    {
        public CharacterAiStateMove(CharacterComponentAi stateManager) :
            base(stateManager)
        {
            type = eType.Move;
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
