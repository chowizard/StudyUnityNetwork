using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client;
using Chowizard.UnityNetwork.Client.Character.Ai;

namespace Chowizard.UnityNetwork.Client.Character
{
    [DisallowMultipleComponent]
    public class CharacterComponentAiNonPlayer : CharacterComponentAi
    {
        public float commandIntervalSeconds = 3.0f;
        public Rect enableMoveBoundary = new Rect(-100.0f, -100.0f, 200.0f, 200.0f);

        private float elapsedCommandTime;
        private bool enableNewCommand;

        private Vector3 startPosition;
        private Quaternion startRotation;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            owner.AddCharacterComponent(this);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

#if BLOCKED
        if(!owner.isServer)
            return;

        UpdateCommandIntervalTime();
        UpdateCommand();
        UpdateAiStateOld();
#endif
        }

        protected override void RegisterAiStates()
        {
            AddAiState(new CharacterAiStateNormalNonPlayer(this));
            AddAiState(new CharacterAiStateCombat(this));
            AddAiState(new CharacterAiStateMove(this));
            AddAiState(new CharacterAiStateReturn(this));

            ChangeAiState(CharacterAiState.eType.Normal, new CharacterAiConditionNormal(owner), new CharacterAiBehaviourStand(owner));
        }

        protected override void UpdateAiState()
        {
            if(!owner.isServer)
                return;

            base.UpdateAiState();
        }

        private void UpdateCommandIntervalTime()
        {
            if(elapsedCommandTime >= commandIntervalSeconds)
            {
                elapsedCommandTime = 0.0f;
                enableNewCommand = true;
            }
            else
            {
                elapsedCommandTime += Time.deltaTime;
                enableNewCommand = false;
            }
        }

        private void UpdateCommand()
        {
            if(enableNewCommand)
            {
                int random = Random.Range(AiStateMinimum, AiStateSize);

                switch((eAiState)random)
                {
                case eAiState.Idle:
                    {
                        CommandIdle();
                    }
                    break;

                case eAiState.Move:
                    {
                        float positionX = Random.Range(enableMoveBoundary.xMin, enableMoveBoundary.xMax);
                        float positionZ = Random.Range(enableMoveBoundary.yMin, enableMoveBoundary.yMax);

                        CommandMove(new Vector3(positionX, transform.position.y, positionZ));
                    }
                    break;

                case eAiState.Rotate:
                    {
                        float angle = Random.Range(0.0f, 360.0f);
                        Quaternion rotation = Quaternion.AngleAxis(angle, owner.transform.localEulerAngles);

                        CommandRotate(rotation);
                    }
                    break;
                }

                elapsedCommandTime = 0.0f;
            }
        }

        private void CommandIdle()
        {
            owner.GetCharacterComponent<CharacterComponentMove>().Stop();
        }

        private void CommandMove(Vector3 targetPosition)
        {
            startPosition = owner.transform.position;
            owner.destinationPosition = targetPosition;

            aiState = eAiState.Move;
        }

        private void CommandRotate(Quaternion targetRotation)
        {
            startRotation = owner.transform.rotation;
            owner.destinationRotation = targetRotation;

            owner.GetCharacterComponent<CharacterComponentMove>().Rotate(owner.destinationRotation);

            aiState = eAiState.Rotate;
        }

        private void UpdateAiStateOld()
        {
            switch(aiState)
            {
            case eAiState.Idle:
                UpdateAiStateIdle();
                break;

            case eAiState.Move:
                UpdateAiStateMove();
                break;

            case eAiState.Rotate:
                UpdateAiStateRotate();
                break;
            }
        }

        private void UpdateAiStateIdle()
        {
        }

        private void UpdateAiStateMove()
        {
            Vector3 distanceStartToDest = owner.destinationPosition - startPosition;
            Vector3 distanceStartToMe = owner.transform.position - startPosition;
            if(distanceStartToMe.sqrMagnitude >= distanceStartToDest.sqrMagnitude)
            {
                owner.transform.position = owner.destinationPosition;
                aiState = eAiState.Idle;
            }
            else if(enableMoveBoundary.Contains(owner.transform.position) == false)
            {
                owner.transform.position = owner.destinationPosition;
                aiState = eAiState.Idle;
            }
            else
            {
                Vector3 distanceMeToDest = owner.destinationPosition - owner.transform.position;
                owner.GetComponent<CharacterComponentMove>().Move(distanceMeToDest.normalized);
            }
        }

        private void UpdateAiStateRotate()
        {
            float delta = Quaternion.Angle(transform.rotation, owner.destinationRotation);
            if(Mathf.Abs(delta) <= 0.001f)
            {
                owner.transform.rotation = owner.destinationRotation;
                aiState = eAiState.Idle;
            }
        }
    }
}
