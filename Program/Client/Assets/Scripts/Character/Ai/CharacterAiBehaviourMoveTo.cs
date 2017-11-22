using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiBehaviourMoveTo : CharacterAiBehaviour
    {
        public Vector3 position;
        public Quaternion rotation;

        public CharacterAiBehaviourMoveTo(CharacterEntity owner) :
            base(eType.MoveTo, owner)
        {
        }

        public CharacterAiBehaviourMoveTo(CharacterEntity owner, Vector3 position, Quaternion rotation) :
            this(owner)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}
