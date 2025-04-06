using System.Collections.Generic;
using UnityEngine;
public class AITransition : MonoBehaviour
{
    private List<AIDecision> decisions;
    public AIState nextState;

    public void SetUp()
    {
        decisions = new();
        GetComponents(decisions);
    }

    public bool MakeATransition()
    {
        bool result = false;
        foreach (var decision in decisions)
        {
            result = decision.MakeADecision();
            if (decision.IsReverse)
            {
                result = !result;
            }

            if (result == false) return false;
        }

        return result;
    }
    
}