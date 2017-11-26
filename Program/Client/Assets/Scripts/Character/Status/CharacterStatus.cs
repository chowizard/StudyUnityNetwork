using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Character.Status
{
    public class CharacterStatus
    {
        public float life;
        public float liftCapacity;
        public float mana;
        public float manaCapacity;

        public float strength;
        public float agility;
        public float vitality;
        public float energy; 

        private CharacterEntity owner;

        public CharacterStatus(CharacterEntity owner)
        {
            this.owner = owner;
        }
    }
}