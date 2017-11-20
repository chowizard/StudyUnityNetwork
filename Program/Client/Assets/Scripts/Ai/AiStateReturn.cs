using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SnowFamily.UnityNet.Client.Ai
{
    public class AiStateReturn : AiState
    {
        public AiStateReturn(CharacterComponentAi stateManager) :
            base(stateManager)
        {
            type = eType.Return;
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
