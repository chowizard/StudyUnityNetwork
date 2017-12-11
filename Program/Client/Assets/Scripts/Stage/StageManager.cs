using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Stage
{
    public sealed class StageManager : MonoBehaviour
    {
        private Dictionary<uint, StageArea> areas = new Dictionary<uint, StageArea>();
        private int capacityX;
        private int capacityY;
        private int capacityZ;

        public StageArea Get(int x, int y, int z)
        {
            return null;
        }

        public void Set(StageArea area, int x, int y, int z)
        {
        }

        public StageArea this[int x, int y, int z]
        {
            get
            {
                return Get(x, y, z);
            }
            set
            {
                Set(value, x, y, z);
            }
        }

        public StageArea[] Areas
        {
            get
            {
                return (areas.Count > 0) ? areas.Values.ToArray() : null;
            }
        }

        public int AreaCount
        {
            get
            {
                return areas.Count;
            }
        }

        private int Horizontal
        {
            get
            {
                return capacityX;
            }
            set
            {
                capacityX = value;
            }
        }

        private int Vertical
        {
            get
            {
                return capacityZ;
            }
            set
            {
                capacityZ = value;
            }
        }

        private int Height
        {
            get
            {
                return capacityY;
            }
            set
            {
                capacityY = value;
            }
        }

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}
