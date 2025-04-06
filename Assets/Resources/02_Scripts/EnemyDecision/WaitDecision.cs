using System;
using UnityEngine;

public class WaitDecision : AIDecision
{
    private float timer = 0;
    public override bool MakeADecision()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            return true;
        }
        return false;
    }
}
