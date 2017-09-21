using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponentMove : MonoBehaviour
{
    public float velocity;

    // Use this for initialization
    void Start()
    {
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");

        float positionX = axisX * velocity * Time.deltaTime;
        float positionZ = axisZ * velocity * Time.deltaTime;

        transform.Translate(axisX, transform.position.y, axisZ);
    }
}
