using System;
using System.Collections.Generic;
using UnityEngine;
public class AIBrain : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rigidbody2D;
    [HideInInspector] public Controlble currentController;
    [HideInInspector] public Controlble targetController;
    [HideInInspector] public Animator animator;
    
    private List<AIState> _aiStates;
    
    [SerializeField] private GameObject enemyUI;
    [SerializeField] private float paddingY;
    [SerializeField] private AIState currentState;
    [SerializeField] public Transform target;
    [HideInInspector] public Transform playerTransform;
    
    [HideInInspector] public int dirX;
    [HideInInspector] public bool skillAble = true;
    
    private WaitState waitState;

    private GameObject statusUI;
    public EnemyStatus enemyStatus;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentController = GetComponent<Controlble>();
        targetController = target.GetComponent<Controlble>();
        playerTransform = target;
        if (currentController is EnemyController)
        {
            Debug.Log(UIManager.Instance);
            statusUI = Instantiate(enemyUI, UIManager.Instance.transform.Find("EnemyStatusParent"));
            enemyStatus = statusUI.GetComponent<EnemyStatus>();
            waitState = transform.Find("EnemyStates").Find("WaitState").GetComponent<WaitState>();
        }

        _aiStates = new List<AIState>();
        GetComponentsInChildren(_aiStates);
        _aiStates.ForEach(state => state.SetUp(transform));
    }

    private void OnEnable()
    {
        if (currentController is EnemyController)
        {
            UIManager.Instance.EnemyStatusEnable(statusUI);
            currentState = waitState;
        }
    }

    private void OnDisable()
    {
        if (currentController is EnemyController)
        {
            UIManager.Instance.EnemyStatusDisable(statusUI);
        }
    }

    private void Update()
    {
        if (animator.GetBool("isDie") == true)  
            return;
        if(currentController is EnemyController)
            UIManager.Instance.EnemyStatusFollowStart(enemyStatus, transform.position, paddingY);
        currentState.OnUpdateState();
        animator.SetFloat("speed", (rigidbody2D.velocity.normalized).x);
        currentController.GroundCheck();
        currentController.DirAnimationCheck(dirX);
    }

    public void ChangeState(AIState state)
    {
        currentState.OnExitState();
        currentState = state;
        currentState.OnEnterState();
    }
}