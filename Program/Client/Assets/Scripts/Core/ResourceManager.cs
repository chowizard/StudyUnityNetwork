using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Core
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance
        {
            get
            {
                if(GameManager.Singleton == null)
                    return null;

                return GameManager.Singleton.resourceManager;
            }
        }

        public GameObject LoadResource(string path)
        {
            return Resources.Load<GameObject>(path);
        }

        public GameObject InstantiateFromResource(string path)
        {
            GameObject prefab = LoadResource(path);
            Debug.Assert(prefab != null);
            if(prefab == null)
                return null;

            return Instantiate(prefab) as GameObject;
        }

        // Use this for initialization
        private void Start()
        {

        }
    }
}
