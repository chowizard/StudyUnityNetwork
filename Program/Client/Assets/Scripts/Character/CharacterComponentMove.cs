using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterComponentMove : CharacterComponent
{
    public void Move(float deltaHorizontal, float deltaVertical)
    {
        float horizontalValue = deltaHorizontal * Velocity * Time.deltaTime;
        float verticalValue = deltaVertical * Velocity * Time.deltaTime;

        if((horizontalValue != 0.0f) || (verticalValue != 0.0f))
            owner.transform.Translate(horizontalValue, owner.transform.position.y, verticalValue);
    }

    // Use this for initialization
    protected override void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {

    }

    public float Velocity
    {
        get
        {
            return owner.moveSpeed;
        }
    }
}
