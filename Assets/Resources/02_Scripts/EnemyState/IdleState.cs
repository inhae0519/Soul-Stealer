using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleState : AIState
{
    private float maxTimer = 3f;
    private float timer;

    private void Awake()
    {
        timer = maxTimer;
    }

    public override void OnEnterState()
    {
        _brain.dirX = Random.Range(-1, 2);
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        base.OnUpdateState();
        Debug.Log("Idle");
        
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            maxTimer = Random.Range(1, 4);
            _brain.dirX = Random.Range(-1, 2);
            timer = maxTimer;
        }
        _brain.rigidbody2D.velocity = new Vector2(_brain.dirX * _brain.currentController.unitSpeed, _brain.rigidbody2D.velocity.y);
    }
}
