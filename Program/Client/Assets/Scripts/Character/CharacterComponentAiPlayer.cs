using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client;
using Chowizard.UnityNetwork.Client.Character.Ai;

namespace Chowizard.UnityNetwork.Client.Character
{
    [DisallowMultipleComponent]
    public class CharacterComponentAiPlayer : CharacterComponentAi
    {
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
            AddAiState(new CharacterAiStateNormal(this));
            AddAiState(new CharacterAiStateCombat(this));
            AddAiState(new CharacterAiStateMove(this));
            AddAiState(new CharacterAiStateReturn(this));
        }
    }
}
