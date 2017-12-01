using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Core
{
    public class GameOptionManager : MonoBehaviour
    {
        public static GameOptionManager Instance
        {
            get
            {
                if(GameManager.Singleton == null)
                    return null;

                return GameManager.Singleton.gameOptionManager;
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
