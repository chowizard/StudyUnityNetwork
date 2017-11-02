using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterComponentMove : CharacterComponent
{
    public void Move(Vector3 direction)
    {
        Vector3 translation = direction * Velocity * Time.deltaTime;

        if(translation != Vector3.zero)
            owner.transform.Translate(translation, Space.World);
    }

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
    }

    public float Velocity
    {
        get
        {
            return owner.moveSpeed;
        }
    }
}
