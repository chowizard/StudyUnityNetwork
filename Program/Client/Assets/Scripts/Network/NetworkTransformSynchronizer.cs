using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class NetworkTransformSynchronizer : NetworkBehaviour
{
    public enum eRotationAxis
    {
        None = 0,

        X,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        XYZ
    }

    public int sendRate;

    public float positionThreshold = 0.01f;
    public float positionInterpolationFactor = 0.01f;

    public eRotationAxis rotationAxis = eRotationAxis.XYZ;
    public float rotationThresholdEulerAngle = 0.01f;
    public float rotationInterpolationFactor = 0.01f;

    [SyncVar]
    private Vector3 position;

    [SyncVar]
    private Vector3 rotationEulerAngles;

    public override int GetNetworkChannel()
    {
        return Channels.DefaultUnreliable;
    }

    public override float GetNetworkSendInterval()
    {
        float sendInterval = 0.0f;
        if(sendRate > 0)
            sendInterval = 1.0f / (float)sendRate;
        else
            base.GetNetworkSendInterval();

        return sendInterval;
    }

    // Use this for initialization
    private void Start()
    {
        position = transform.position;
        rotationEulerAngles = transform.localEulerAngles;
    }

    // Update is called once per frame
    private void Update()
    {
        if(isLocalPlayer == true)
            return;

        UpdateInterpolatePosition();
        UpdateInterpolateRotation();
    }

    private void FixedUpdate()
    {
        if(isLocalPlayer == true)
        {
            bool isPositionChanged = IsPositionChanged;
            bool isRotationChanged = IsRotationChanged;

            if((isPositionChanged == true) && (isRotationChanged == true))
            {
                CmdMoveTransform(transform.position, transform.localEulerAngles);
                position = transform.position;
                rotationEulerAngles = transform.localEulerAngles;
            }
            else
            {
                if(isPositionChanged == true)
                {
                    CmdMovePosition(transform.position);
                    position = transform.position;
                }

                if(isRotationChanged == true)
                {
                    CmdMoveRotation(transform.localEulerAngles);
                    rotationEulerAngles = transform.localEulerAngles;
                }
            }
        }

        if(isServer == true)
        {
            bool isPositionChanged = IsPositionChanged;
            bool isRotationChanged = IsRotationChanged;

            if((isPositionChanged == true) && (isRotationChanged == true))
            {
                RpcMoveTransform(transform.position, transform.localEulerAngles);
                position = transform.position;
                rotationEulerAngles = transform.localEulerAngles;
            }
            else
            {
                if(isPositionChanged == true)
                {
                    RpcMovePosition(transform.position);
                    position = transform.position;
                }

                if(isRotationChanged == true)
                {
                    RpcMoveRotation(transform.localEulerAngles);
                    rotationEulerAngles = transform.localEulerAngles;
                }
            }
        }
    }

    private void UpdateInterpolatePosition()
    {
        Vector3 previousPosition = transform.position;
        float delta = positionInterpolationFactor * Time.deltaTime;

        transform.position = Vector3.Lerp(previousPosition, position, delta);
    }

    private void UpdateInterpolateRotation()
    {
        Quaternion nowRotation = Quaternion.Euler(rotationEulerAngles);
        float delta = rotationInterpolationFactor * Time.deltaTime;

        transform.rotation = Quaternion.Lerp(transform.rotation, nowRotation, delta);
    }

    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdMovePosition(Vector3 position)
    {
        this.position = position;
    }

    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdMoveRotation(Vector3 rotationEulerAngles)
    {
        this.rotationEulerAngles = rotationEulerAngles;
    }

    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdMoveTransform(Vector3 position, Vector3 rotationEulerAngles)
    {
        this.position = position;
        this.rotationEulerAngles = rotationEulerAngles;
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    private void RpcMovePosition(Vector3 position)
    {
        this.position = position;
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    private void RpcMoveRotation(Vector3 rotationEulerAngles)
    {
        this.rotationEulerAngles = rotationEulerAngles;
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    private void RpcMoveTransform(Vector3 position, Vector3 rotationEulerAngles)
    {
        this.position = position;
        this.rotationEulerAngles = rotationEulerAngles;
    }

    private bool IsPositionChanged
    {
        get
        {
            float positionThresholdSqr2 = positionThreshold * positionThreshold;
            float distanceSqr2 = Vector3.SqrMagnitude(transform.position - position);
            return (distanceSqr2 > positionThresholdSqr2) ? true : false;
        }
    }

    private bool IsRotationChanged
    {
        get
        {
            float rotationThresholdEulerAngleSqr2 = rotationThresholdEulerAngle * rotationThresholdEulerAngle;
            float distanceSqr2 = Vector3.SqrMagnitude(transform.localEulerAngles - rotationEulerAngles);
            return (distanceSqr2 > rotationThresholdEulerAngleSqr2) ? true : false;
        }
    }
}
