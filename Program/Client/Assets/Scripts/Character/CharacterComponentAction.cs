using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Character.Action;

namespace Chowizard.UnityNetwork.Client.Character
{
    [DisallowMultipleComponent]
    public class CharacterComponentAction : CharacterComponent
    {
        public CharacterAction currentAction;
        public CharacterAction nextAction;

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

            UpdateAction();
        }

        protected void UpdateAction()
        {
            if(nextAction != null)
            {
                currentAction = nextAction;
            }
        }
    }
}

