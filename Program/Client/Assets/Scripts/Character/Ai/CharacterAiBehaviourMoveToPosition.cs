using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public class CharacterAiBehaviourMoveToPosition : CharacterAiBehaviour
    {
        public Vector3 position;
        public Quaternion rotation;

        public CharacterAiBehaviourMoveToPosition(CharacterEntity owner) :
            base(eType.MoveTo, owner)
        {
        }

        public CharacterAiBehaviourMoveToPosition(CharacterEntity owner, Vector3 position, Quaternion rotation) :
            this(owner)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
}
