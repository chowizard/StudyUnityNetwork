using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Utility
{
    public class GizmoDrawer : MonoBehaviour
    {
        public enum eShape
        {
            None = 0,

            Sphere,
            WireSphere,
            Cube,
            WireCube,
        }

        public eShape shapeMode = eShape.None;
        public bool displaySelectedOnly;

        [Space]
        public float radius;
        public Vector3 size;
        public Color color = Color.white;

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void OnDrawGizmos()
        {
            if(displaySelectedOnly == true)
                return;

            Gizmos.color = color;

            switch(shapeMode)
            {
            case eShape.Sphere:
                Gizmos.DrawSphere(transform.position, radius);
                break;

            case eShape.WireSphere:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;

            case eShape.Cube:
                Gizmos.DrawCube(transform.position, size);
                break;

            case eShape.WireCube:
                Gizmos.DrawWireCube(transform.position, size);
                break;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if(displaySelectedOnly == false)
                return;

            Gizmos.color = color;

            switch(shapeMode)
            {
            case eShape.Sphere:
                Gizmos.DrawSphere(transform.position, radius);
                break;

            case eShape.WireSphere:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;

            case eShape.Cube:
                Gizmos.DrawCube(transform.position, size);
                break;

            case eShape.WireCube:
                Gizmos.DrawWireCube(transform.position, size);
                break;
            }
        }
    }
}
