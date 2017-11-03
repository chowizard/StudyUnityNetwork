using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionManager : MonoBehaviour
{
    public static GameOptionManager Instance
    {
        get
        {
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
