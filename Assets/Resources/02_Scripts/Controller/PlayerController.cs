using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : Controlble
{
    public bool haveKey;
    private CapsuleCollider2D hitBox;

    protected override void Awake()
    {
        base.Awake();
        hitBox = transform.Find("HitBox").GetComponent<CapsuleCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
        if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            unitSoulLevel = GameManager.Instance.defaultLevel;
            unitHp = GameManager.Instance.defaultHp;
            unitCurrentHp = GameManager.Instance.defaultCurrentHp;
            unitAtk = GameManager.Instance.defaultAtk;
        }
        else
        {
            unitSoulLevel = GameManager.Instance.GetLevelStat();
            unitHp = GameManager.Instance.GetHpStat();
            unitCurrentHp = GameManager.Instance.GetCurrentHpStat();
            unitAtk = GameManager.Instance.GetAtkStat();
        }
        CameraManager.Instance.CameraFollowChange(transform);
        UIManager.Instance.StartUpdateStatus(AgentDictionaly.Instance.UnitDataTypeSoDic[unitName], this);
    }

    public override void OnSelect(InputAction.CallbackContext context)
    {
        AIBrain nextAiBrain;
        Controlble nextEnemyController;
        
        base.OnSelect(context);
        nextAiBrain = currentSelectUnit.GetComponent<AIBrain>();
        nextAiBrain.enabled = false;
        nextEnemyController = currentSelectUnit.GetComponent<Controlble>();
        nextEnemyController.enabled = true;
    }

    public override void TakeDamage(float damage)
    {
        unitCurrentHp -= damage;
        Debug.Log(unitCurrentHp);
        if (isControlling)
        {
            AudioManager.Instance.PlayEffect(damageClip);
            UIManager.Instance.StartUpdateStatus(AgentDictionaly.Instance.UnitDataTypeSoDic[unitName], this);
        }
        else
            UIManager.Instance.EnablePlayerStatus(AgentDictionaly.Instance.UnitDataTypeSoDic[unitName], this);
        StartCoroutine(AttackEffect());
        
        if(unitCurrentHp<0)
            Die();
    }

    public override void Die()
    {
        base.Die();
        UIManager.Instance.GameOverPanelOn();
    }

    protected override IEnumerator AttackEffect()
    {
        hitBox.enabled = false;
        spriteRenderer.material = hitMat;
        
        yield return new WaitForSeconds(0.3f);
        
        spriteRenderer.material = defaultMat;
        hitBox.enabled = true;
    }

    private void FixedUpdate()
    {
        Move();
    }
}
        