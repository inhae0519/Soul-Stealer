using System;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private AudioClip charge;
    [SerializeField] private AudioClip laserSound;
    private Controlble controlble;
    private AIBrain aiBrain;
    private GameObject laser;
    private Collider2D skillBound;
    private Animator animator;
    private float castTime;
    public float castComTime;
    public float castEndTime;
    private void Awake()
    {
        controlble = transform.root.GetComponent<Controlble>();
        aiBrain = transform.root.GetComponent<AIBrain>();
        animator = transform.Find("Laser").GetComponent<Animator>();
        skillBound = transform.Find("LaserBound").GetComponent<Collider2D>();
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        castTime = 0;
    }

    private void FixedUpdate()
    {
        castTime += Time.fixedDeltaTime;
        AudioManager.Instance.LaserSound(charge);
        if (castTime >= castComTime)
        {
            AudioManager.Instance.LaserSoundStop();
            AudioManager.Instance.LaserSound(laserSound);
            animator.SetBool("CastComplete", true);
        }
        
        if(castTime >= castComTime+0.1f)
            skillBound.enabled = true;

        if (castTime >= castEndTime)
        {
            AudioManager.Instance.LaserSoundStop();
            animator.SetBool("CastComplete", false);
            skillBound.enabled = false;
        }

        if (castTime >= castEndTime + 1f)
        {
            controlble.isAttack = false;
            gameObject.SetActive(false);
        }
    }
}
    
