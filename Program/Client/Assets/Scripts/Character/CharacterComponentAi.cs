using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Character.Ai;

namespace Chowizard.UnityNetwork.Client.Character
{
    public abstract class CharacterComponentAi : CharacterComponent
    {
        public enum eAiState
        {
            Idle = 0,
            Move,
            Rotate,
        }

        public const int AiStateMinimum = (int)eAiState.Idle;
        public const int AiStateMaximum = (int)eAiState.Rotate;
        public const int AiStateSize = AiStateMaximum + 1;

        public const int DefaultUpdateAiStateRate = 30;


        public eAiState aiState = eAiState.Idle;

        public Dictionary<CharacterAiState.eType, CharacterAiState> aiStates = new Dictionary<CharacterAiState.eType, CharacterAiState>();
        public CharacterAiState currentAiState;
        public CharacterAiState nextAiState;

        public int updateAiRatePerSeconds = DefaultUpdateAiStateRate;
        private float elapsedUpdateAiTime;

        public virtual void ChangeAiState(CharacterAiState.eType type, CharacterAiCondition condition, CharacterAiBehaviour behaviour)
        {
            nextAiState = GetAiState(type);
            Debug.Assert(nextAiState != null);

            Debug.Assert(condition != null);
            nextAiState.Condition = condition;
            Debug.Assert(behaviour != null);
            nextAiState.Behaviour = behaviour;

            currentAiState.Exit();
        }

        public CharacterAiState GetAiState(CharacterAiState.eType type)
        {
            CharacterAiState data;
            return aiStates.TryGetValue(type, out data) ? data : null;
        }

        public bool IsExistNextAiState
        {
            get
            {
                return (nextAiState != null) ? true : false;
            }
        }

        protected abstract void RegisterAiStates();

        protected virtual void UpdateAiState()
        {
            if(currentAiState == null)
                return;

            if(nextAiState != null)
            {
                currentAiState = nextAiState;
                nextAiState = null;

                currentAiState.Enter();
            }
            else
            {
                currentAiState.Update();
            }
        }

        protected virtual float GetUpdateAiTimeInterval()
        {
            if(updateAiRatePerSeconds <= 0)
                return 0.0f;

            float updateTimeInterval = 1.0f / (float)updateAiRatePerSeconds;
            return updateTimeInterval;
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            float updateTimeInterval = GetUpdateAiTimeInterval();
            if((updateTimeInterval <= 0.0f) || (elapsedUpdateAiTime >= updateTimeInterval))
            {
                UpdateAiState();
                elapsedUpdateAiTime = 0.0f;
            }
            else
            {
                elapsedUpdateAiTime += Time.deltaTime;
            }
        }

        protected void ClearAiStates()
        {
            aiStates.Clear();
        }

        protected void AddAiState(CharacterAiState aiState)
        {
            Debug.Assert(aiState != null);
            Debug.Assert(aiState.Type != CharacterAiState.eType.None);
            Debug.Assert(aiStates.ContainsKey(aiState.Type) == false);

            aiStates.Add(aiState.Type, aiState);
        }
    }
}

