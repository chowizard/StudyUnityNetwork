using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Character;

namespace Chowizard.UnityNetwork.Client.Network
{
    public class NetworkTransformSynchronizer : NetworkBehaviour
    {
        public enum eSyncType
        {
            None = 0,

            Transform,
            Rigidbody2D,
            Rigidbody3D
        }

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

        public eSyncType syncType = eSyncType.Transform;
        [Range(min: 1, max: 30)]
        public int sendRate;
        public int channel = Channels.DefaultUnreliable;

        [Range(min: 0.0f, max: 100.0f)]
        public float positionThreshold = 0.01f;

        [Range(min: 0.01f, max: 100.0f)]
        public float positionInterpolationFactor = 0.01f;

        [Range(min: 0.01f, max: 100.0f)]
        public float positionSnapThreshold = 1.0f;

        private float elapsedTimeReceivedPosition;
        //private float interpolationRatio;

        public eRotationAxis rotationAxis = eRotationAxis.XYZ;

        [Range(min: 0.0f, max: 360.0f)]
        public float rotationThreshold = 0.01f;

        [Range(min: 0.01f, max: 100.0f)]
        public float rotationInterpolationFactor = 0.01f;

        [Range(min: 0.01f, max: 360.0f)]
        public float rotationSnapThreshold = 1.0f;

        private float elapsedTimeReceivedRotation;

        [SyncVar]
        private Vector3 position;

        [SyncVar]
        private Vector3 rotationEulerAngles;

        public override int GetNetworkChannel()
        {
            return channel;
        }

        public override float GetNetworkSendInterval()
        {
            float sendInterval = 0.0f;
            if(sendRate > 0)
                sendInterval = 1.0f / (float)sendRate;
            else
                sendInterval = base.GetNetworkSendInterval();

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
            UpdateSynchonizationType();

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

        private void UpdateSynchonizationType()
        {
            switch(syncType)
            {
            case eSyncType.None:
                {
                    if(enabled == true)
                        enabled = false;
                }
                break;

            case eSyncType.Transform:
                {
                    if(enabled == false)
                        enabled = true;
                }
                break;

            case eSyncType.Rigidbody2D:
                {
                    if(enabled == false)
                        enabled = true;
                }
                break;

            case eSyncType.Rigidbody3D:
                {
                    if(enabled == false)
                        enabled = true;
                }
                break;
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

        [Command]
        private void CmdMovePosition(Vector3 position)
        {
            this.position = position;
        }

        [Command]
        private void CmdMoveRotation(Vector3 rotationEulerAngles)
        {
            this.rotationEulerAngles = rotationEulerAngles;
        }

        [Command]
        private void CmdMoveTransform(Vector3 position, Vector3 rotationEulerAngles)
        {
            this.position = position;
            this.rotationEulerAngles = rotationEulerAngles;
        }

        [ClientRpc]
        private void RpcMovePosition(Vector3 position)
        {
            this.position = position;
            elapsedTimeReceivedPosition = 0.0f;
        }

        [ClientRpc]
        private void RpcMoveRotation(Vector3 rotationEulerAngles)
        {
            this.rotationEulerAngles = rotationEulerAngles;
            elapsedTimeReceivedRotation = 0.0f;
        }

        [ClientRpc]
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
}

