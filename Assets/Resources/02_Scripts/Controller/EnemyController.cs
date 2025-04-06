using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class EnemyController : Controlble
{
    [SerializeField] private GameObject soul;
    private GameObject states;
    private GameObject statusUI;
    private AIBrain aiBrain;

    protected override void Awake()
    {
        base.Awake();
        aiBrain = GetComponent<AIBrain>(); 
    }

    protected override void Start()
    {
        base.Start();
        UIManager.Instance.EnemyStatusUpdateStart(aiBrain.enemyStatus , this);
        states = transform.Find("EnemyStates").gameObject;
        aiBrain.enabled = true;
        enabled = false;
    }

    public override void DirAnimationCheck(float dir)
    {
        if (isAttack)
            return;
        base.DirAnimationCheck(dir);
    }

    public override void OnSelect(InputAction.CallbackContext context)
    {
        AIBrain nextAiBrain;
        EnemyController nextEnemyController;
        GameObject nextStates;
        
        base.OnSelect(context);
        
        enabled = false;
        aiBrain.enabled = true;
        states.SetActive(true);

        if (currentSelectUnit.CompareTag("Enemy"))
        {
            nextEnemyController = currentSelectUnit.GetComponent<EnemyController>();
            nextEnemyController.enabled = true;
            
            nextAiBrain = currentSelectUnit.GetComponent<AIBrain>();
            nextAiBrain.enabled = false;
            
            nextStates = currentSelectUnit.transform.Find("EnemyStates").gameObject;
            nextStates.SetActive(false);
        }
    }

    public void OnReturnPlayer(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        
        ReturnPlayer();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        animator.SetTrigger("damaged");
        UIManager.Instance.EnemyStatusUpdateStart(aiBrain.enemyStatus , this);
        Vector2 hitDir = aiBrain.target.position - transform.position;

        if (hitDir.x > 0 && !isControlling)
        {
            transform.localScale = Vector3.one;
        }
        else if (hitDir.x < 0 && !isControlling)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
    }

    public override void Die()
    {
        base.Die();
        Destroy(aiBrain.enemyStatus.gameObject);
        GameObject soulGameObject = Instantiate(soul, transform.position, quaternion.identity);
        soulGameObject.GetComponent<Soul>().soulIndex = unitSoulLevel;
        ReturnPlayer();
    }

    private void ReturnPlayer()
    {
        if (!isControlling)
            return;
        Controlble playerController = playerInput.GetComponent<Controlble>();
        
        currentInputSystem.enabled = false;
        playerInput.enabled = true;
        isControlling = false;
        enabled = false;
        playerController.isControlling = true;
        
        aiBrain.enabled = true;
        states.SetActive(true);
        
        UIManager.Instance.StartUpdateStatus(AgentDictionaly.Instance.UnitDataTypeSoDic["Player"], playerController);
        UIManager.Instance.DisablePlayerStatus();
        CameraManager.Instance.CameraFollowChange(aiBrain.playerTransform);
    }

    private void FixedUpdate()
    {
        Move();
    }
}
