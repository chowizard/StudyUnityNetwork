using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class CharacterComponentAi : CharacterComponent
{
    public enum eAiState
    {
        Idle = 0,
        Move,
        Rotate,
    }

    public const int AiStateMinimum = (int)eAiState.Idle;
    public const int AiStateMaximum = (int)eAiState.Rotate;
    public const int AiStateSize = AiStateMaximum + 1;

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
