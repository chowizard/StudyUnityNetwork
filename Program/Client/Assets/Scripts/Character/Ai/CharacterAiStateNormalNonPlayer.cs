using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public sealed class CharacterAiStateNormalNonPlayer : CharacterAiStateNormal
    {
        public float CommandIntervalSeconds = 1.0f;
        private float elapsedCommandTime;



        public CharacterAiStateNormalNonPlayer(CharacterComponentAi stateManager) :
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

            if(elapsedCommandTime >= CommandIntervalSeconds)
            {
                SelectNextBehaviour();
                elapsedCommandTime = 0.0f;
            }
            else
            {
                elapsedCommandTime += Time.deltaTime;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        /* 다음 행동을 결정한다. */
        private void SelectNextBehaviour()
        {
            int random = Random.Range(0, 100);
            if(random < 33)
            {
                /* 이동할 지점과 회전할 지점을 정한다. */
                float movePositionX = Random.Range(Owner.transform.position.x - 5.0f, Owner.transform.position.x + 5.0f);
                float movePositionZ = Random.Range(Owner.transform.position.z - 5.0f, Owner.transform.position.z + 5.0f);
                Vector3 movePosition = new Vector3(movePositionX, Owner.transform.position.y, movePositionZ);

                float angle = Random.Range(0.0f, 360.0f);
                Quaternion rotation = Quaternion.AngleAxis(angle, Owner.transform.localEulerAngles);

                CharacterAiConditionNormal condition = new CharacterAiConditionNormal(Owner);
                CharacterAiBehaviourMoveToPosition behaviour = new CharacterAiBehaviourMoveToPosition(Owner, movePosition, rotation);

                /* 그 쪽으로 이동시킨다. */
                stateManager.ChangeAiState(eType.Move, condition, behaviour);
            }
            else if(random < 66)
            {
                float angle = Random.Range(0.0f, 360.0f);
                Quaternion rotation = Quaternion.AngleAxis(angle, Owner.transform.localEulerAngles);

                CharacterAiConditionNormal condition = new CharacterAiConditionNormal(Owner);
                CharacterAiBehaviourMoveToPosition behaviour = new CharacterAiBehaviourMoveToPosition(Owner, Owner.transform.position, rotation);

                /* 그 쪽으로 이동시킨다. */
                stateManager.ChangeAiState(eType.Move, condition, behaviour);
            }
            else
            {
                CharacterAiConditionNormal condition = new CharacterAiConditionNormal(Owner);
                CharacterAiBehaviourStand behaviour = new CharacterAiBehaviourStand(Owner);

                stateManager.ChangeAiState(eType.Normal, condition, behaviour);
            }
        }
    }
}
