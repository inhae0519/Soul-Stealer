using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheckDecision : AIDecision
{
    public override bool MakeADecision()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x + aiBrain.dirX, transform.position.y), Vector2.down, 5f, aiBrain.currentController.groundLayer);
    }
}
