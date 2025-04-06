using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class BossSkillState : AIState
{
    [SerializeField] private Laser laser;
    [SerializeField] private Transform[] points;
    private GameObject laserObj;
    private int skillIndex;
    private BossController bossController;

    private void Start()
    {
        bossController = _brain.currentController as BossController;
        laserObj = laser.gameObject;
    }

    public override void OnEnterState()
    {
        skillIndex = Random.Range(0, 4);
    }

    public override void OnExitState()
    {
    }

    public override void OnUpdateState()
    {
        base.OnUpdateState();
        Debug.Log("Skill");
        if (_brain.currentController.unitCurrentHp / _brain.currentController.unitHp >= 0.5f)
        {
            if (_brain.skillAble)
            {
                if (skillIndex <= 1)
                {
                    Debug.Log("Skill1");
                    _brain.skillAble = false;
                    StartCoroutine(bossController.Skill1Routine());
                }
                else
                {
                    Debug.Log("Skill2");
                    _brain.skillAble = false;
                    laser.castComTime = 1f;
                    laser.castEndTime = 3f;
                    StartCoroutine(bossController.Skill2Routine());
                }
            }
        }
        else
        {
            if (_brain.skillAble)
            {
                if (skillIndex <= 1)
                {
                    Debug.Log("Skill1");
                    _brain.skillAble = false;
                    if (skillIndex == 0)
                    {
                        _brain.dirX = 1;
                    }
                    else
                    {
                        _brain.dirX = -1;
                    }
                    transform.root.DOMove(points[skillIndex].position, 0.5f)
                        .OnComplete(() => StartCoroutine(bossController.Skill1Routine()));
                }
                else
                {
                    Debug.Log("Skill2");
                    _brain.skillAble = false;
                    laser.castComTime = 0f;
                    laser.castEndTime = 3f;

                    if (skillIndex == 2)
                    {
                        _brain.dirX = 1;
                    }
                    else
                    {
                        _brain.dirX = -1;
                    } 
                    _brain.rigidbody2D.gravityScale = 0f;
                    transform.root.DOMove(points[skillIndex].position, 1f).OnComplete(() =>
                    {
                        StartCoroutine(bossController.Skill2Routine());
                    });
                    laserObj.transform.localRotation = quaternion.Euler(0,0,45);
                    StartCoroutine(OnWaitRoutine());
                    laserObj.transform.DOLocalRotate(new Vector3(0,0,-90), 5f).OnComplete(()=>
                    {
                        _brain.rigidbody2D.gravityScale = 2;
                        laserObj.transform.localRotation = quaternion.Euler(0,0,0);
                    });
                }
            }
        }
    }

    private IEnumerator OnWaitRoutine()
    {
        yield return new WaitForSeconds(2f);
    }
}
