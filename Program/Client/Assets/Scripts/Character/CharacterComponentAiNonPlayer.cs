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
        }

        protected override void RegisterAiStates()
        {
            AddAiState(new CharacterAiStateNormalNonPlayer(this));
            AddAiState(new CharacterAiStateCombat(this));
            AddAiState(new CharacterAiStateMoveToPosition(this));
            AddAiState(new CharacterAiStateReturn(this));

            ChangeAiState(CharacterAiState.eType.Normal, new CharacterAiConditionNormal(owner), new CharacterAiBehaviourStand(owner));
        }

        protected override void UpdateAiState()
        {
            if(!owner.isServer)
                return;

            base.UpdateAiState();
        }
    }
}
