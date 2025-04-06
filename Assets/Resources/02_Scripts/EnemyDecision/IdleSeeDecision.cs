using UnityEngine;

public class IdleSeeDecision : AIDecision
{
    [SerializeField] private float viewAngle;
    private Vector2 viewDir;
    private float viewHalfAngle;

    protected override void Awake()
    {
        base.Awake();
        viewHalfAngle = viewAngle / 2;
    }
    
    public override bool MakeADecision()
    {
        Vector3 dir = (aiBrain.target.transform.position - transform.position).normalized;
        if (transform.root.localScale.x <= -1)
        {
            viewDir = new Vector2(-1, 0);
        }
        else
        {
            viewDir = new Vector2(1, 0);
        }
        float dot = Vector2.Dot(dir, viewDir);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (angle <= viewHalfAngle)
        {   
            return true;
        }
        return false;
    }
}