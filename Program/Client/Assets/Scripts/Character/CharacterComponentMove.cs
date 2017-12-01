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
            Start,
            Moving,
            End
        }

        public enum eRotateState
        {
            NotRotate,
            Start,
            Rotating,
            End,
        }

        private eMoveState moveState = eMoveState.Stopped;
        private Vector3 startPosition;
        private Vector3 destinationPosition;

        private eRotateState rotateState = eRotateState.NotRotate;
        private Quaternion targetRotation;
        private float elapsedTimeToRotate;

        public void Stop()
        {
            StopMove();
            StopRotate();
        }

        public void StopMove()
        {
            owner.destinationPosition = owner.transform.position;

            moveState = eMoveState.End;
        }

        public void StopRotate()
        {
            owner.destinationRotation = owner.transform.rotation;
            targetRotation = owner.transform.rotation;
            EndRotation();

            rotateState = eRotateState.End;
        }

        public void MoveToPosition(Vector3 position)
        {
            startPosition = owner.transform.position;
            destinationPosition = position;

            moveState = eMoveState.Start;
        }

        public void MoveToDirection(Vector3 direction)
        {
            if(direction == Vector3.zero)
                return;

            Vector3 translation = (direction * owner.moveSpeed * Time.deltaTime);
            if(translation == Vector3.zero)
                return;

            startPosition = owner.transform.position;
            destinationPosition = owner.transform.position + translation;

            moveState = eMoveState.Start;
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

        public bool IsMoving
        {
            get
            {
                return (moveState == eMoveState.Moving) ? true : false;
            }
        }

        public Vector3 MoveDirection
        {
            get
            {
                return Vector3.Normalize(destinationPosition - startPosition);
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
                return elapsedTimeToRotate;
            }
        }

        public bool IsRotating
        {
            get
            {
                return (rotateState == eRotateState.Rotating) ? true : false;
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
            UpdateRotate();
        }

        private void UpdateMove()
        {
            switch(moveState)
            {
            case eMoveState.Stopped:
                UpdateMoveStateStopped();
                break;

            case eMoveState.Start:
                UpdateMoveStateStarted();
                break;

            case eMoveState.Moving:
                UpdateMoveStateMoving();
                break;

            case eMoveState.End:
                UpdateMoveStateArrived();
                break;
            }
        }

        private void UpdateMoveStateStopped()
        {
        }

        private void UpdateMoveStateStarted()
        {
            moveState = eMoveState.Moving;
        }

        private void UpdateMoveStateMoving()
        {
            float distanceStartToDistinationSqr2 = Vector3.SqrMagnitude(destinationPosition - startPosition);
            float distanceStartToSelfSqr2 = Vector3.SqrMagnitude(owner.transform.position - startPosition);

            if(distanceStartToSelfSqr2 >= distanceStartToDistinationSqr2)
            {
                StopMove();
            }
            else
            {
                Vector3 translation = MoveDirection * MoveSpeed * Time.deltaTime;

                if(translation != Vector3.zero)
                    owner.transform.Translate(translation, Space.World);
            }
        }

        private void UpdateMoveStateArrived()
        {
            moveState = eMoveState.Stopped;
        }

        private void UpdateRotate()
        {
            switch(rotateState)
            {
            case eRotateState.NotRotate:
                UpdateRotateNotRotate();
                break;

            case eRotateState.Start:
                UpdateRotateStart();
                break;

            case eRotateState.Rotating:
                UpdateRotateRotating();
                break;

            case eRotateState.End:
                UpdateRotateEnd();
                break;
            }
        }

        private void UpdateRotateNotRotate()
        {
        }

        private void UpdateRotateStart()
        {
            rotateState = eRotateState.Rotating;
        }

        private void UpdateRotateRotating()
        {
            float interpolation = owner.rotationSpeed * elapsedTimeToRotate;
            if(interpolation >= 1.0f)
            {
                StopRotate();
            }
            else
            {
                Quaternion.Lerp(transform.rotation, targetRotation, interpolation);
                elapsedTimeToRotate += Time.deltaTime;
            }
        }

        private void UpdateRotateEnd()
        {
            EndRotation();

            rotateState = eRotateState.NotRotate;
        }

        private void EndRotation()
        {
            elapsedTimeToRotate = 0.0f;
        }
    }
}

