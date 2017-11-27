using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Character
{
    [DisallowMultipleComponent]
    public class CharacterComponentMove : CharacterComponent
    {
        public enum eMoveState
        {
            Stopped = 0,
            Started,
            Moving,
            Arrived
        }

        public enum eRotateState
        {
            NotRotate,
            Started,
            Rotated,
            Complete,
        }

        private eMoveState moveState = eMoveState.Stopped;
        private Vector3 startPosition;
        private Vector3 destinationPosition;

        private eRotateState rotateState = eRotateState.NotRotate;
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

        public void MoveToPosition(Vector3 position)
        {
            startPosition = owner.transform.position;
            destinationPosition = position;


        }

        public void MoveToDirection(Vector3 direction)
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

        public eMoveState MoveState
        {
            get
            {
                return moveState;
            }
        }

        public Vector3 StartPosition
        {
            get
            {
                return startPosition;
            }
        }

        public Vector3 DestinationPosition
        {
            get
            {
                return destinationPosition;
            }
        }

        public eRotateState RotateState
        {
            get
            {
                return rotateState;
            }
        }

        public Quaternion TargetRotation
        {
            get
            {
                return targetRotation;
            }
        }

        public float ElapsedTimeRotation
        {
            get
            {
                return elapsedTimeRotation;
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

            UpdateMove();
            UpdateRotation();
        }

        private void UpdateMove()
        {
            switch(moveState)
            {
            case eMoveState.Stopped:
                UpdateMoveStateStopped();
                break;

            case eMoveState.Started:
                UpdateMoveStateStarted();
                break;

            case eMoveState.Moving:
                UpdateMoveStateMoving();
                break;

            case eMoveState.Arrived:
                UpdateMoveStateArrived();
                break;
            }
        }

        private void UpdateMoveStateStopped()
        {
        }

        private void UpdateMoveStateStarted()
        {
        }

        private void UpdateMoveStateMoving()
        {
            //Vector3 distanceStartToDistinationSqr2 = Vector3.SqrMagnitude();
            //Vector3 distanceSelfToDestinationSqr2 = Vector3.SqrMagnitude();
        }

        private void UpdateMoveStateArrived()
        {
            moveState = eMoveState.Stopped;
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
}

