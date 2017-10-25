using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public Vector3 velocity;
    public float speed = 4.0f;

    private Rigidbody selfRigidbody;

    // Use this for initialization
    private void Start()
    {
        selfRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckVelocity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hittedTarget = collision.gameObject;
        PlayerCharacter moveComponent = hittedTarget.GetComponent<PlayerCharacter>();
        if(moveComponent != null)
            Destroy(gameObject);
    }

    private void CheckVelocity()
    {
        Vector3 currentVelocity = velocity * speed;

        if(selfRigidbody.velocity != currentVelocity)
            selfRigidbody.velocity = currentVelocity;
    }
}
