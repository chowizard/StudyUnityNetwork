using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiStateMoveToPosition : CharacterAiState
    {
        private bool isEndToMove;
        private bool isEndToRotate;

        public CharacterAiStateMoveToPosition(CharacterComponentAi stateManager) :
            base(stateManager)
        {
            type = eType.Move;
        }

        public override void Enter()
        {
            base.Enter();

            isEndToMove = false;
            isEndToRotate = false;
        }

        public override void Update()
        {
            base.Update();

            if(isEndToRotate == false)
            {
                /* 먼저 회전이 끝나야 위치 이동을 할 수 있다. */
                UpdateRotate();
                return;
            }

            if(isEndToMove == false)
                UpdateMove();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool IsEndToMove
        {
            get
            {
                return isEndToMove;
            }
        }

        public bool IsEndToRotate
        {
            get
            {
                return isEndToRotate;
            }
        }

        private void UpdateMove()
        {
            CharacterAiBehaviourMoveToPosition detailBehaviour = behaviour as CharacterAiBehaviourMoveToPosition;
            Debug.Assert(detailBehaviour != null);

            //detailBehaviour.rotation
        }

        private void UpdateRotate()
        {

        }
    }
}
