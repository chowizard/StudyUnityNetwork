using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Trigger
{
    [DisallowMultipleComponent]
    public class TriggerArea : MonoBehaviour
    {
        public TriggerManager manager;
        public uint triggerId;

        public Vector3 minimum;
        public Vector3 maximum;
        private BoxCollider boundary;

        public bool IsContain(Vector3 position)
        {
            return false;
        }

        private void Awake()
        {
        }

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void RefreshBoundary()
        {
        }

        private void GenerateBoundary()
        {
        }
    }
}

