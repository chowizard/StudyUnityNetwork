using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public abstract class CharacterAiBehaviour
    {
        public enum eType
        {
            None = 0,

            Stand,
            MoveTo,
            Chase,
            Attack,
            ReturnTo,
        }

        private eType type = eType.None;
        private CharacterEntity owner;

        public CharacterAiBehaviour(eType type, CharacterEntity owner)
        {
            this.type = type;
            this.owner = owner;
        }

        public eType Type
        {
            get
            {
                return type;
            }
        }

        public CharacterEntity Owner
        {
            get
            {
                return owner;
            }
        }
    }
}
