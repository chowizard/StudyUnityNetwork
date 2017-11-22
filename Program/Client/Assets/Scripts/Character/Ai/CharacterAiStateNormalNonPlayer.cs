using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public sealed class CharacterAiStateNormalNonPlayer : CharacterAiStateNormal
    {
        public float CommandIntervalSeconds = 3.0f;
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
            if(random < 50)
            {
                CharacterAiStateMove aiStateMove = stateManager.GetAiState(eType.Move) as CharacterAiStateMove;
                Debug.Assert(aiStateMove != null);

                //aiStateMove.
            }
            else
            {
            }
        }
    }
}
