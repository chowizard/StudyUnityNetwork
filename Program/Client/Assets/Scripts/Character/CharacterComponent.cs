using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    public enum eState
    {
        None = 0,

        Active,
        Deactive,
    }

    public eState state = eState.None;

    protected CharacterEntity owner;

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
