using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : AIState
{
    public override void OnEnterState()
    {
    }

    public override void OnExitState()
    {
    }

    public override void OnUpdateState()
    {
        base.OnUpdateState();
        Debug.Log("Chase");
    }
}
