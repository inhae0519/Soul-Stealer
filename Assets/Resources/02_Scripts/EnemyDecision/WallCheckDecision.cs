using UnityEngine;
using System;

public class WallCheckDecision : AIDecision
{
    private RangeDecision rangeDecision;
    protected override void Awake()
    {
        base.Awake();
        rangeDecision = GetComponent<RangeDecision>();
    }

    public override bool MakeADecision()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(transform.root.localScale.x, 0), rangeDecision.range);
        Array.Sort(hits, (a, b) => 
                (a.distance.CompareTo(b.distance)));
        foreach (var hit in hits)
        {
            if (hit.transform == transform.root)
            {
                continue;
            }
            
            if (hit.collider.gameObject.tag == "Ground")
                return false;
            
            else if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Player")
                return true;
        }
        return false;
    }
}