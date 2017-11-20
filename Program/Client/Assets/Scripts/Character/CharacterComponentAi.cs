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
    public AiState currentAiState;
    public AiState nextAiState;

    public int updateAiRatePerSeconds;
    private float elapsedUpdateAiTime;

    public virtual void ChangeAiState(AiState.eType aiState)
    {
        nextAiState = GetAiState(aiState);
        Debug.Assert(nextAiState != null);

        currentAiState.Exit();
    }

    public AiState GetAiState(AiState.eType type)
    {
        AiState data;
        return aiStates.TryGetValue(type, out data) ? data : null;
    }

    protected virtual void UpdateAiState()
    {
        if(currentAiState == null)
            return;

        if(nextAiState != null)
        {
            currentAiState = nextAiState;
            nextAiState = null;

            currentAiState.Enter();
        }
        else
        {
            currentAiState.Update();
        }
    }

    protected virtual float GetUpdateAiTimeInterval()
    {
        if(updateAiRatePerSeconds <= 0)
            return 0.0f;

        float updateTimeInterval = 1.0f / (float)updateAiRatePerSeconds;
        return updateTimeInterval;
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

        float updateTimeInterval = GetUpdateAiTimeInterval();
        if((updateTimeInterval <= 0.0f) || (elapsedUpdateAiTime >= updateTimeInterval))
        {
            UpdateAiState();
            elapsedUpdateAiTime = 0.0f;
        }
        else
        {
            elapsedUpdateAiTime += Time.deltaTime;
        }
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
