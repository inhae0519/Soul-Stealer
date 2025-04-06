using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimeAndDamageDecision : AIDecision
{
    private float maxTimer;
    private float timer;
    private void Start()
    {
        maxTimer = Random.Range(4, 7);
        Debug.Log(maxTimer);
    }

    public override bool MakeADecision()
    {
        timer += Time.deltaTime;
        Debug.Log(timer);
        if (timer >= maxTimer)
        {
            timer = 0;
            return true;
        }
        return false;
    }
}
