using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class CharacterComponentInputControl : CharacterComponent
{

    // Use this for initialization
    protected override void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
        if(networkIdentity == null)
            return;

        if(networkIdentity.isServer == true)
            UpdateServer();

        if(networkIdentity.isClient == true)
            UpdateClient();
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
                moveComponent.Move(axisX, axisZ);
        }
    }
}
