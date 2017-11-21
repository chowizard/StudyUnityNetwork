using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Chowizard.UnityNetwork.Client.Core;

namespace Chowizard.UnityNetwork.Client.Character
{
    [DisallowMultipleComponent]
    public class CharacterComponentNetworkSensor : CharacterComponent
    {
        public enum eSensorLevel
        {
            Normal = 0,
            Close,
            Far,
            NotSeen
        }

        public SphereCollider sensorNormal;
        public SphereCollider sensorClose;
        public SphereCollider sensorFar;

        public eSensorLevel currentSensorLevel;
        public float sensorDistanceLimitNormal = 10.0f;
        public float sensorDistanceLimitClose = 2.0f;
        public float sensorDistanceLimitFar = 20.0f;

        public int syncRateNormal = 10;
        public int syncRateClose = 15;
        public int syncRateFar = 5;

        public float sensorTickSeconds = 1.0f;
        private float elapsedSendorTickSeconds;

        // Use this for initialization
        protected override void Start()
        {

        }

        // Update is called once per frame
        protected override void Update()
        {
            UpdateSensorLevel();
        }

        private void OnCollisionEnter(Collision collision)
        {

        }

        private void OnCollisionExit(Collision collision)
        {

        }

        private void UpdateSensorLevel()
        {
            if(elapsedSendorTickSeconds < sensorTickSeconds)
            {
                elapsedSendorTickSeconds += Time.deltaTime;
                return;
            }

            elapsedSendorTickSeconds = 0.0f;

            if(EntityManager.Instance.PlayerCount <= 0)
                return;

            //float nearestDistance;
            //foreach(CharacterEntity player in EntityManager.Instance.Players)
            //{
            //    if(player == null)
            //        continue;

            //    SphereCollider collider = player.sensor.GetComponent<SphereCollider>();
            //    if(collider == null)
            //        continue;

            //    //collider.radius;
            //}
        }
    }
}
