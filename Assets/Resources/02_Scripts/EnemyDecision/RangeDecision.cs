using UnityEngine;

public class RangeDecision : AIDecision
{
    public float range;
    public override bool MakeADecision()
    {
        return Vector2.Distance(transform.position, aiBrain.target.position) <= range;
    }
}
