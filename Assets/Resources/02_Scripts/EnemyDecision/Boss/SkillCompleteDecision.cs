using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCompleteDecision : AIDecision
{
    public override bool MakeADecision()
    {
        return aiBrain.currentController.isAttack;
    }
}
