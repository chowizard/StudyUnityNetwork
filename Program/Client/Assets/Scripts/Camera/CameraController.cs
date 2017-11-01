using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public GameObject followTarget;
    public bool isFollowTarget;

    public void Move(Vector3 translate)
    {
        if(camera == null)
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
    }

    private void LateUpdate()
    {
        UpdateFollowing();
    }

    private void UpdateFollowing()
    {
        if((isFollowTarget == true) && (followTarget != null))
        {
            //Vector3 translation = transform.position - followTarget.transform.position;
            //Vector3 translation = followTarget.transform.position;
            //if(translation != Vector3.zero)
            //    Move(translation);
            if(transform.position != followTarget.transform.position)
                transform.position = followTarget.transform.position;
        }
    }
}
