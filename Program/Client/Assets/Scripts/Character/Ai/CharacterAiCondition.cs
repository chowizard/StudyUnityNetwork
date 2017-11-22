using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Ai
{
    public abstract class CharacterAiCondition
    {
        public enum eType
        {
            None = 0,

            Normal,
            Dead,
            SensedEnemy,
        }

        private eType type = eType.None;

        public CharacterAiCondition(eType type)
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
