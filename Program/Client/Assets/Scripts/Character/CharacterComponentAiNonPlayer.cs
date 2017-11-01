using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterComponentAiNonPlayer : CharacterComponentAi
{
    public float commandIntervalSeconds = 3.0f;

    private float elapsedCommandTime;

    private Vector3 startPosition;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        owner.AddCharacterComponent(this);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(elapsedCommandTime >= commandIntervalSeconds)
        {
            float positionX = Random.Range(-100.0f, 100.0f);
            float positionZ = Random.Range(-100.0f, 100.0f);

            CommandMove(new Vector3(positionX, transform.position.y, positionZ));
            elapsedCommandTime = 0.0f;
        }
        else
        {
            elapsedCommandTime += Time.deltaTime;
        }

        UpdateAiState();
    }

    private void UpdateAiState()
    {
        switch(aiState)
        {
        case eAiState.Idle:
            UpdateAiStateIdle();
            break;

        case eAiState.Move:
            UpdateAiStateMove();
            break;
        }
    }

    private void CommandMove(Vector3 targetPosition)
    {
        startPosition = owner.transform.position;
        owner.destinationPosition = targetPosition;

        aiState = eAiState.Move;
    }

    private void UpdateAiStateIdle()
    {
    }

    private void UpdateAiStateMove()
    {
        Vector3 distanceStartToDest = owner.destinationPosition - startPosition;
        Vector3 distanceStartToMe = owner.transform.position - startPosition;
        if(distanceStartToMe.sqrMagnitude >= distanceStartToDest.sqrMagnitude)
        {
            owner.transform.position = owner.destinationPosition;
            aiState = eAiState.Idle;
        }
        else
        {
            Vector3 distanceMeToDest = owner.destinationPosition - owner.transform.position;
            owner.GetComponent<CharacterComponentMove>().Move(distanceMeToDest.normalized);
        }
    }
}
