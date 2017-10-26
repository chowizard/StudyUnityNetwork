using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    public enum eActionState
    {
        None = 0,

        Idle,
        Death,
        Move,
        Attack,
    }

    public eActionState currentActionState = eActionState.None;
    public Vector3 destinationPosition;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void UpdateMove()
    {

    }

    private void MoveTo(Vector3 destination)
    {

    }
}
