using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character
{
    [RequireComponent(typeof(CharacterEntity))]
    public abstract class CharacterComponent : MonoBehaviour
    {
        public enum eState
        {
            None = 0,

            Active,
            Deactive,
        }

        public eState state = eState.None;

        public CharacterEntity owner;

        // Use this for initialization
        protected virtual void Start()
        {
            if(owner == null)
                owner = GetComponent<CharacterEntity>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // 내용 없음
        }
    }
}

