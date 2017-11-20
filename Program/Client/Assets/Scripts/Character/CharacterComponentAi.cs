using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SnowFamily.UnityNet.Client.Ai;

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

    public Dictionary<AiState.eType, AiState> aiStates = new Dictionary<AiState.eType, AiState>();
    public AiState.eType currentAiState = AiState.eType.None;
    public AiState.eType nextAiState = AiState.eType.None;

    public virtual void ChangeAiState(AiState.eType aiState)
    {
        nextAiState = aiState;
    }

    public AiState GetAiState(AiState.eType type)
    {
        AiState data;
        return aiStates.TryGetValue(type, out data) ? data : null;
    }

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

    protected void ClearAiStates()
    {
        aiStates.Clear();
    }

    protected void AddAiState(AiState aiState)
    {
        Debug.Assert(aiState != null);
        Debug.Assert(aiState.Type != AiState.eType.None);
        Debug.Assert(aiStates.ContainsKey(aiState.Type) == false);

        aiStates.Add(aiState.Type, aiState);
    }
}
