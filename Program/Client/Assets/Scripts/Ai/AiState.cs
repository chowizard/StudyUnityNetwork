using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SnowFamily.UnityNet.Client.Ai
{
    public abstract class AiState
    {
        public enum eType
        {
            None = 0,

            Normal,
            Move,
            Combat,
            Return
        }

        public enum eState
        {
            None = 0,

            Enter,
            Update,
            Exit,
        }

        protected eType type = eType.None;
        protected eState state = eState.None;
        protected CharacterComponentAi stateManager;

        public AiState(CharacterComponentAi stateManager)
        {
            this.stateManager = stateManager;
        }

        public virtual void Enter()
        {
            state = eState.Enter;
        }

        public virtual void Update()
        {
            state = eState.Update;
        }

        public virtual void Exit()
        {
            state = eState.Exit;
        }

        public eType Type
        {
            get
            {
                return type;
            }
        }

        public eState State
        {
            get
            {
                return state;
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
    }
}
