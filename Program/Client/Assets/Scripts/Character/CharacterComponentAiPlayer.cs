using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterComponentAiPlayer : CharacterComponentAi
{
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
    }
}
