using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : NPC
{
    protected override void IntroUpdate()
    {
        StepToDestination(counterPosition);
        base.IntroUpdate();
    }

    protected override void WaitingUpdate()
    {
        StepToDestination(chairPosition);
        base.WaitingUpdate();
    }
}
