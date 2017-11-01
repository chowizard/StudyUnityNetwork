using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class CharacterComponentAi : CharacterComponent
{
    public enum eAiState
    {
        Idle = 0,
        Move
    }

    public eAiState aiState = eAiState.Idle;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
