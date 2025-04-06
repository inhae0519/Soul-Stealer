using UnityEngine;

public class AttackState : AIState
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
        Debug.Log("Attack");
        _brain.dirX = Mathf.RoundToInt((_brain.target.position - transform.position).normalized.x);
        if (_brain.currentController.isAttack)
            return;
        if (_brain.targetController.isDie == true)
        {
            _brain.target = _brain.playerTransform;
        }
        StartCoroutine(_brain.currentController.Attack());
    }
}