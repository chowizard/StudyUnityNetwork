using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public abstract class CharacterAiState
    {
        public enum eType
        {
            None = 0,

            Normal,
            Move,
            Combat,
            Return
        }

        public enum eProcessState
        {
            None = 0,

            Enter,
            Update,
            Exit,
        }

        public const int StateTypeMinimum = (int)eType.Normal;
        public const int StateTypeMaximum = (int)eType.Return;
        public const int StateTypeSize = StateTypeMaximum + 1;


        protected eType type = eType.None;
        protected eProcessState processState = eProcessState.None;
        protected CharacterComponentAi stateManager;

        protected CharacterAiCondition condition;
        protected CharacterAiBehaviour behaviour;

        public CharacterAiState(CharacterComponentAi stateManager)
        {
            this.stateManager = stateManager;
        }

        public virtual void Enter()
        {
            processState = eProcessState.Enter;
        }

        public virtual void Update()
        {
            processState = eProcessState.Update;
        }

        public virtual void Exit()
        {
            processState = eProcessState.Exit;
        }

        public eType Type
        {
            get
            {
                return type;
            }
        }

        public eProcessState State
        {
            get
            {
                return processState;
            }
        }

        public CharacterComponentAi StateManager
        {
            get
            {
                return stateManager;
            }
        }

        public CharacterEntity Owner
        {
            get
            {
                return (stateManager != null) ? stateManager.owner : null;
            }
        }

        public CharacterAiCondition Condition
        {
            get
            {
                return condition;
            }
        }

        public CharacterAiBehaviour Behaviour
        {
            get
            {
                return behaviour;
            }
        }
    }
}
