using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class AIState : MonoBehaviour, IState
{
    protected List<AITransition> transitions;
    protected AIBrain _brain;

    public virtual void SetUp(Transform agent)
    {
        _brain = agent.GetComponent<AIBrain>();
        transitions = new();
        GetComponentsInChildren(transitions);
        
        transitions.ForEach(transition=>transition.SetUp());
    }

    public abstract void OnEnterState();

    public abstract void OnExitState();

    public virtual void OnUpdateState()
    {
        foreach (var transition in transitions)
        {
            if (transition.MakeATransition())
            {
                _brain.ChangeState(transition.nextState);
            }
        }
    }
}