using UnityEngine;

public class WaitState : AIState
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
        Debug.Log("Waiting");
        _brain.rigidbody2D.velocity = new Vector2(0, _brain.rigidbody2D.velocity.y);
    }
}
