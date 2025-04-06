using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossController : Controlble
{
    [SerializeField] private AudioClip stoneSound;
    [SerializeField] private GameObject bossCam;
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject text;
    private GameObject states;
    private AIBrain aiBrain;
    private Laser laser;
    private bool skill1Able = true;
    private bool skill2Able = true;
    public bool active = true;
    private float skill1Delay = 4f;
    private float skill2Delay = 6f;

    protected override void Awake()
    {
        base.Awake();
        laser = transform.Find("Skill2").GetComponent<Laser>();
        aiBrain = GetComponent<AIBrain>();
    }

    protected override void Start()
    {
        base.Start();
        states = transform.Find("EnemyStates").gameObject;
        aiBrain.enabled = true;
        enabled = false;
        UIManager.Instance.BossStatusUpdateStart( this);
    }

    public void OnSkill1(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        StartCoroutine(Skill1Routine());
    }
    
    public void OnSkill2(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        laser.castComTime = 1f;
        laser.castEndTime = 2f;
        StartCoroutine(Skill2Routine());
    }

    public IEnumerator Skill1Routine()
    {
        if (skill1Able)
        {
            AudioManager.Instance.PlayEffect(stoneSound);
            unitAtk = defaultAtk + 5f;
            isAttack = true;
            skill1Able = false;
            animator.SetTrigger("Skill1");
            yield return new WaitForSeconds(skill1Delay);
            aiBrain.skillAble = true;
            skill1Able = true;
        }
    }
    
    public IEnumerator Skill2Routine()
    {
        if (skill2Able)
        {
            unitAtk = defaultAtk + 8f;
            isAttack = true;
            skill2Able = false;
            animator.SetTrigger("Skill2");
            yield return new WaitForSeconds(skill2Delay);
            aiBrain.skillAble = true;
            skill2Able = true;
        }
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
        if(isControlling && unitCurrentHp<=0)
            ReturnPlayer();
        base.TakeDamage(damage);
        animator.SetTrigger("damaged");
        UIManager.Instance.BossStatusUpdateStart(this);
    }

    public override void Die()
    {
        DeActive();
    }

    public void DeActive()
    {
        active = false;
        animator.SetBool("deactive", true);
        UIManager.Instance.DisableBossStatus();
        lever.SetActive(true);
        bossCam.SetActive(false);
        unitCurrentHp = unitHp;
        text.SetActive(true);
        aiBrain.enabled = false;
        isAttack = false;
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
        animator.SetBool("isDie", true);
        Destroy(gameObject, 3f);
        
        UIManager.Instance.StartUpdateStatus(AgentDictionaly.Instance.UnitDataTypeSoDic["Player"], playerController);
        UIManager.Instance.DisablePlayerStatus();
        CameraManager.Instance.CameraFollowChange(aiBrain.playerTransform);
    }

    private void FixedUpdate()
    {
        Move();
    }
    
}
