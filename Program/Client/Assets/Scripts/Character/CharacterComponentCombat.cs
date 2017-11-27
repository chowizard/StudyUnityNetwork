using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client;

namespace Chowizard.UnityNetwork.Client.Character
{
	[DisallowMultipleComponent]
	public class CharacterComponentCombat : CharacterComponent
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
	}
}
