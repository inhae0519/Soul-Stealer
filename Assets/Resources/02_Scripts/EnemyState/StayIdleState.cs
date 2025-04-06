using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayIdleState : AIState
{
    private Vector3 returnPos;
    private Vector3 dir;
    private void Awake()
    {
        returnPos = transform.position;
    }
    public override void OnEnterState()
    {
    }

    public override void OnExitState()
    {
    }
    
    public override void OnUpdateState()
    {
        base.OnUpdateState();
        Debug.Log("StayIdle");
        dir = returnPos - transform.position;
        if (Mathf.RoundToInt(transform.position.x) == Mathf.RoundToInt(returnPos.x))
            _brain.dirX = 0;
        else if (dir.x > 0)
            _brain.dirX = 1;
        else if (dir.x < 0)
            _brain.dirX = -1;
        _brain.rigidbody2D.velocity = new Vector2(_brain.dirX * _brain.currentController.unitSpeed, _brain.rigidbody2D.velocity.y);
    }
}
