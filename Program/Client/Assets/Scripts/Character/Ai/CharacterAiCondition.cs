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
        private CharacterEntity owner;

        public CharacterAiCondition(eType type, CharacterEntity owner)
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
