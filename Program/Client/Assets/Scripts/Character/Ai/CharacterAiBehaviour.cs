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

        public CharacterAiBehaviour(eType type)
        {
            this.type = type;
        }

        public eType Type
        {
            get
            {
                return type;
            }
        }
    }
}
