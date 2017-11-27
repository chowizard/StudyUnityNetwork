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
            /* 아직 회전이 완료가 안 된 상태라면 이동이 불가능함. */
            if(isEndToRotate == false)
            {
                Debug.LogError(string.Format("Character {0} rotation not ended yet!", Owner.netId));
                return;
            }

            CharacterAiBehaviourMoveToPosition detailBehaviour = behaviour as CharacterAiBehaviourMoveToPosition;
            Debug.Assert(detailBehaviour != null);

            CharacterComponentMove moveComponent = Owner.GetCharacterComponent<CharacterComponentMove>();
            Debug.Assert(moveComponent != null);
            if(moveComponent == null)
                return;

            //moveComponent.Move();
        }

        private void UpdateRotate()
        {
            CharacterAiBehaviourMoveToPosition detailBehaviour = behaviour as CharacterAiBehaviourMoveToPosition;
            Debug.Assert(detailBehaviour != null);

            /* 이미 회전이 완료가 된 상태라면 종료함. */
            if(Owner.transform.rotation == detailBehaviour.rotation)
            {
                isEndToRotate = true;
                return;
            }

            CharacterComponentMove moveComponent = Owner.GetCharacterComponent<CharacterComponentMove>();
            Debug.Assert(moveComponent != null);
            if(moveComponent == null)
                return;

            /* 회전각이 안 맞는 상태이므로, 목표 회전각이 될 때까지 회전하도록 한다. */
            if(moveComponent.isStartedRotation == false)
                moveComponent.Rotate(detailBehaviour.rotation);
        }
    }
}
