using UnityEngine;

public class ChaseState : AIState
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
        _brain.dirX = Mathf.RoundToInt((_brain.target.position - transform.position).normalized.x);
        if (_brain.currentController.isAttack)
            return;
        _brain.rigidbody2D.velocity = new Vector2(_brain.dirX * _brain.currentController.unitSpeed, _brain.rigidbody2D.velocity.y);
    }
}