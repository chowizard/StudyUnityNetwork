using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera targetCamera;
    public GameObject followTarget;
    public bool isFollowTarget;

    public float moveSpeed = 10.0f;

    public void Move(Vector3 translate)
    {
        if(targetCamera == null)
            return;

        transform.Translate(translate);
    }

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if(followTarget == false)
            UpdateCameraPosition();
    }

    private void LateUpdate()
    {
        if(followTarget == true)
            UpdateFollowing();
    }

    private void UpdateCameraPosition()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");

        if((axisX != 0.0f) || (axisZ != 0.0f))
        {
            float velocityX = axisX * moveSpeed * Time.deltaTime;
            float velocityZ = axisZ * moveSpeed * Time.deltaTime;
            Vector3 velocity = new Vector3(velocityX, transform.position.y, velocityZ);

            transform.Translate(velocity);
        }
    }

    private void UpdateFollowing()
    {
        if(isFollowTarget == false)
            return;

        if(followTarget == null)
            return;

        if(transform.position != followTarget.transform.position)
            transform.position = followTarget.transform.position;
    }
}
