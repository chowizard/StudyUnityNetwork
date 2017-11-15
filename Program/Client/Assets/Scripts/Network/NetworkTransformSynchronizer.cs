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
    public float positionSnapThreshold = 1.0f;
    private float elapsedTimeReceivedPosition;
    //private float interpolationRatio;

    public eRotationAxis rotationAxis = eRotationAxis.XYZ;
    public float rotationThreshold = 0.01f;
    public float rotationInterpolationFactor = 0.01f;
    public float rotationSnapThreshold = 1.0f;
    private float elapsedTimeReceivedRotation;

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

    public override void OnNetworkDestroy()
    {
    }

    // Use this for initialization
    private void Start()
    {
        position = transform.position;
        rotationEulerAngles = transform.localEulerAngles;

        positionInterpolationFactor = GetComponent<CharacterEntity>().moveSpeed;
        //positionInterpolationFactor = 1.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if(hasAuthority == false)
        {
            UpdateInterpolatePosition();
            UpdateInterpolateRotation();
        }
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
        float distanceSqr2 = Vector3.SqrMagnitude(position - previousPosition);

        float delta;
        if(distanceSqr2 >= positionSnapThreshold * positionSnapThreshold)
            delta = 1.0f;
        else
            delta = positionInterpolationFactor * elapsedTimeReceivedPosition;

        transform.position = Vector3.Lerp(previousPosition, position, delta);

        elapsedTimeReceivedPosition += Time.deltaTime;
    }

    private void UpdateInterpolateRotation()
    {
        Quaternion nowRotation = Quaternion.Euler(rotationEulerAngles);
        float distanceSqr2 = Vector3.SqrMagnitude(rotationEulerAngles - transform.localEulerAngles);

        float delta;
        if((rotationInterpolationFactor <= 0.0f) ||
           (distanceSqr2 >= rotationSnapThreshold * rotationSnapThreshold))
        {
            delta = 1.0f;
        }
        else
        {
            delta = rotationInterpolationFactor * elapsedTimeReceivedRotation;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, nowRotation, delta);

        elapsedTimeReceivedRotation += Time.deltaTime;
    }

    //[Command(channel = Channels.DefaultReliable)]
    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdMovePosition(Vector3 position)
    {
        this.position = position;
    }

    //[Command(channel = Channels.DefaultReliable)]
    [Command(channel = Channels.DefaultUnreliable)]
    private void CmdMoveRotation(Vector3 rotationEulerAngles)
    {
        this.rotationEulerAngles = rotationEulerAngles;
    }

    //[Command(channel = Channels.DefaultReliable)]
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
        elapsedTimeReceivedPosition = 0.0f;
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    private void RpcMoveRotation(Vector3 rotationEulerAngles)
    {
        this.rotationEulerAngles = rotationEulerAngles;
        elapsedTimeReceivedRotation = 0.0f;
    }

    [ClientRpc(channel = Channels.DefaultUnreliable)]
    private void RpcMoveTransform(Vector3 position, Vector3 rotationEulerAngles)
    {
        this.position = position;
        elapsedTimeReceivedPosition = 0.0f;

        this.rotationEulerAngles = rotationEulerAngles;
        elapsedTimeReceivedRotation = 0.0f;
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
            float rotationThresholdEulerAngleSqr2 = rotationThreshold * rotationThreshold;
            float distanceSqr2 = Vector3.SqrMagnitude(transform.localEulerAngles - rotationEulerAngles);
            return (distanceSqr2 > rotationThresholdEulerAngleSqr2) ? true : false;
        }
    }
}
