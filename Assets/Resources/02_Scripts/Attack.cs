using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private AudioClip audioClip;
    private Collider2D attackBound;
    private GameObject effect;
    private GameObject skill;
    private Controlble controlble;
    private AIBrain aiBrain;

    private void Awake()
    {
        controlble = GetComponent<Controlble>();
        aiBrain = GetComponent<AIBrain>();
        audioClip = Resources.Load<AudioClip>("03_Sprites/HitSound");
        attackBound = transform.Find("Attack").Find("AttackBound").GetComponent<Collider2D>();
        if(controlble is BossController)
            skill = transform.Find("Skill2").gameObject;
        if(controlble is EnemyController)
            effect = transform.Find("Attack").Find("Effect").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && tag == "Player" || other.CompareTag("Player") && tag == "Enemy" || other.CompareTag("Enemy") && tag == "Enemy")
        {
            AIBrain targetAIBrain = other.GetComponent<AIBrain>();
            Controlble targetController = other.transform.root.GetComponent<Controlble>();
            // 몬스터 조종 상태로 플레이어 공격 불가
            if (other.CompareTag("Player") && controlble.isControlling)
                return;
            // 몬스터끼리 공격 불가
            if (other.CompareTag("Enemy") && !controlble.isControlling && !targetController.isControlling && other.transform != aiBrain.target)
                return;
            
            if(controlble.isControlling)
                AudioManager.Instance.PlayEffect(audioClip);

            if (other.CompareTag("Enemy") && tag == "Enemy" && !targetController.isControlling && (controlble.isControlling || controlble.damagedFriendly))
            {
                Debug.Log("count");
                targetController.damagedFriendly = true;
                targetAIBrain.target = transform;
                targetAIBrain.targetController = GetComponent<Controlble>();
            }
            
            
            if (targetController.unitCurrentHp > 0)
            {
                if(controlble is EnemyController)
                    effect.SetActive(true);
                targetController.TakeDamage(controlble.unitAtk);
            }
        }
        
        else if (other.CompareTag("Hit_Interactable") && controlble.isControlling)
        {
            AudioManager.Instance.PlayEffect(audioClip);
            other.GetComponent<Hit_Interactable>().TakeDamage();
        }
    }

    public void EnableAttackBound()
    {
        attackBound.enabled = true;
    }

    public void DisableAttackBound()
    {
        attackBound.enabled = false;
    }

    public void EnableSkill()
    {
        skill.SetActive(true);
    }
}