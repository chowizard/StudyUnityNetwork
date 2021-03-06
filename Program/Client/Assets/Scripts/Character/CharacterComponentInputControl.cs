﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace Chowizard.UnityNetwork.Client.Character
{
    [DisallowMultipleComponent]
    public class CharacterComponentInputControl : CharacterComponent
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

            NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
            if(networkIdentity == null)
                return;

            if(Network.NetworkManager.Instance.mode == Network.NetworkManager.eMode.None)
            {
                UpdateClient();
            }
            else
            {
                if(networkIdentity.isServer == true)
                    UpdateServer();

                if(networkIdentity.isClient == true)
                    UpdateClient();
            }
        }

        private void UpdateServer()
        {
        }

        private void UpdateClient()
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisZ = Input.GetAxis("Vertical");

            if((axisX != 0.0f) || (axisZ != 0.0f))
            {
                CharacterComponentMove moveComponent = owner.GetCharacterComponent<CharacterComponentMove>();
                if(moveComponent != null)
                    moveComponent.MoveToDirection(new Vector3(axisX, 0.0f, axisZ));
            }
        }
    }
}

