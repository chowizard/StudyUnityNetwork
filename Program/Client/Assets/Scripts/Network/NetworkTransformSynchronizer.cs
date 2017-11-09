using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class NetworkTransformSynchronizer : NetworkBehaviour
{
    public int sendRate;
    public float positionThreshold;
    public float positionInterpolationFactor;
    public float rotationThresholdEuler;
    public float rotationInterpolationFactor;

    [SyncVar]
    private Vector3 position;

    [SyncVar]
    private Quaternion rotation;


    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        UpdateInterpolatePosition();
        UpdateInterpolateRotation();
    }

    private void FixedUpdate()
    {
        if(sendRate <= 0)
            return;


    }

    private void UpdateInterpolatePosition()
    {
    }

    private void UpdateInterpolateRotation()
    {
    }
}
