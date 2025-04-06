using UnityEngine;

[RequireComponent(typeof(AITransition))]
public abstract class AIDecision : MonoBehaviour
{
    public bool IsReverse;
    protected AIBrain aiBrain;

    protected virtual void Awake()
    {
        aiBrain = transform.root.GetComponent<AIBrain>();
    }

    public abstract bool MakeADecision();
}