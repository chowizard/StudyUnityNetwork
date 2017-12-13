using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Stage
{
    [DisallowMultipleComponent]
    public abstract class StageArea : MonoBehaviour
    {
        public StageManager manager;
        public uint id;

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
            RefreshBoundary();
        }

        private void RefreshBoundary()
        {
        }

        private void GenerateBoundary()
        {
        }
    }
}

