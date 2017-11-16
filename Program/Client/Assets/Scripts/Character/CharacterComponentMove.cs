using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[DisallowMultipleComponent]
public class CharacterComponentMove : CharacterComponent
{
    private Quaternion targetRotation;
    private float elapsedTimeRotation;
    public bool isStartedRotation;

    public void Stop()
    {
        owner.destinationPosition = owner.transform.position;

        owner.destinationRotation = owner.transform.rotation;
        targetRotation = owner.transform.rotation;
        EndRotation();
    }

    public void Move(Vector3 direction)
    {
        Vector3 translation = direction * MoveSpeed * Time.deltaTime;

        if(translation != Vector3.zero)
            owner.transform.Translate(translation, Space.World);
    }

    public void Rotate(Quaternion rotation)
    {
        bool isEqualRotation = (owner.transform.rotation == rotation) ? true : false;
        bool existRotationSpeed = (owner.rotationSpeed > 0.0f) ? true : false;

        if((isEqualRotation == true) || (existRotationSpeed == false))
        {
            owner.transform.rotation = rotation;
            EndRotation();
        }
        else
        {
            targetRotation = rotation;
            isStartedRotation = true;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return owner.moveSpeed;
        }
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

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        float interpolation = owner.rotationSpeed * elapsedTimeRotation;
        Quaternion.Lerp(transform.rotation, targetRotation, interpolation);

        elapsedTimeRotation += Time.deltaTime;
    }

    private void EndRotation()
    {
        elapsedTimeRotation = 0.0f;
        isStartedRotation = false;
    }
}
