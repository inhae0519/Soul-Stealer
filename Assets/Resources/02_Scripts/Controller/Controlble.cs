using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Controlble : Agent
{
    private LayerMask swapAbleLayer;
    [HideInInspector] public LayerMask groundLayer;
    
    [SerializeField] private bool debugMod;
    [Range(0f, 360f)]
    [SerializeField] private float viewAngle;
    private float viewHalfAngle;
    [Range(-180f, 180f)] [SerializeField] private float viewRotateZ;   
    
    private float moveDirection;
    [SerializeField] float jumpCheckDistance = 2f;
    private float bodySwapRange = 5f;
    private float distance;
    private float closestDistance = 6f;

    private Vector2 vignetteCenter;
    private float vignetteCenterX = 0.5f;
    private float vignetteCenterY = 0.5f;

    private bool isSwap;
    protected bool isJump;
    public bool isAttack;
    public bool isControlling;
    public bool isDie;
    public bool damagedFriendly;
    private bool attackAble = true;

    private Rigidbody2D _rigidbody2D;
    private Collider2D[] inRangeMonsterCol;
    private Collider2D mostCloseMonster = null;
    private GameObject currentSelectMark = null;

    protected Animator animator;
    protected Material hitMat;
    protected Material defaultMat;
    protected SpriteRenderer spriteRenderer;
    protected GameObject currentSelectUnit;
    protected PlayerInput currentInputSystem;
    protected PlayerInput swapInputSystem;
    protected static PlayerInput playerInput;

    protected float defaultAtk;
    protected AudioClip damageClip;
    
    protected virtual void Awake()
    {
        hitMat = Resources.Load<Material>("07_Mat/HitMat");
        damageClip = Resources.Load<AudioClip>("03_Sprites/DamageSound");
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentInputSystem = GetComponent<PlayerInput>();
        
        if (gameObject.GetComponent<Controlble>() is PlayerController)
            playerInput = currentInputSystem;


        defaultMat = spriteRenderer.material;
        swapAbleLayer = LayerMask.GetMask("BodySwapAble");
        groundLayer = LayerMask.GetMask("Ground");
        viewHalfAngle = viewAngle * 0.5f;
    }

    protected override void Start()
    {
        base.Start();
        defaultAtk = unitAtk;
    }

    protected virtual void Update()
    {
        isJump = !GroundCheck();
        FallingCheck();
        DirAnimationCheck(_rigidbody2D.velocity.normalized.x);
    }

    private void OnEnable()
    {
        isDie = false;
        attackAble = true;
    }

    public virtual void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed)
            return;
        moveDirection = context.ReadValue<float>();
        if (moveDirection == 1)
            vignetteCenterX = 0.55f;
        
        else if (moveDirection == -1)
            vignetteCenterX = 0.45f;
        
        else
            vignetteCenterX = 0.5f;
    }
    protected virtual void Move()
    {
        if (isAttack)
            return;
        _rigidbody2D.velocity = new Vector2(unitSpeed * moveDirection, _rigidbody2D.velocity.y);
        animator.SetFloat("speed", (_rigidbody2D.velocity.normalized).x);
    }
    
    public virtual void DirAnimationCheck(float dir)
    {
        if (dir<0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else if(dir>0)
        {
            transform.localScale = Vector3.one;
        }
    }

    public virtual void OnJump(InputAction.CallbackContext context)
    {
        if(!context.started)
            return;
        if (!isJump)
        {
            vignetteCenterY = 0.55f;
            animator.SetBool("isJump", true);
            _rigidbody2D.AddForce(unitJumpPower * Vector2.up, ForceMode2D.Impulse);
        }
    }
    public bool GroundCheck()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, jumpCheckDistance, groundLayer);
    }

    public void FallingCheck()
    {
        if (_rigidbody2D.velocity.y <= -0.1f)
        {
            vignetteCenterY = 0.5f;
            animator.SetBool("isJump", false);
        }
        animator.SetBool("isGround", GroundCheck());
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if(!context.started)
            return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3f, LayerMask.GetMask("Interactable"));
        if (hit != null)
        {
            print("안디");
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            interactable.BaseInteract();
        }
        else
            return;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(!context.started || isJump)
            return;
        StartCoroutine(Attack());
    }

    public virtual IEnumerator Attack()
    {
        if (attackAble)
        {
            isAttack = true;
            attackAble = false;
            animator.SetTrigger("attacking");
            yield return new WaitForSeconds(unitAtkDelay);
            attackAble = true;
        }
    }

    public void OffIsAttack()
    {
        isAttack = false;
    }

    public virtual void TakeDamage(float damage)
    {
        unitCurrentHp -= damage;
        if (isControlling)
        {
            AudioManager.Instance.PlayEffect(damageClip);
            UIManager.Instance.StartUpdateStatus(AgentDictionaly.Instance.UnitDataTypeSoDic[unitName], this);
        }
        if (isAttack)
            StartCoroutine(AttackEffect());
        
        if(unitCurrentHp<=0)
            Die();
    }
    
    protected virtual IEnumerator AttackEffect()
    {
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.material = defaultMat;
    }

    public virtual void Die()
    {
        if (isDie)
            return;
        Debug.Log(gameObject.name + "Die");
        Collider2D attackBound = transform.Find("Attack").Find("AttackBound").GetComponent<Collider2D>();
        attackBound.enabled = false;
        isDie = true;
        animator.SetBool("isDie", true);
        StartCoroutine(GameObjectDisable());
    }

    private IEnumerator GameObjectDisable()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    public void OnTryBodySwap(InputAction.CallbackContext context)
    {
        if(!context.started || CameraManager.Instance.closing || isAttack)
            return;
        inRangeMonsterCol = Physics2D.OverlapCircleAll(transform.position, bodySwapRange, swapAbleLayer);
        int count = inRangeMonsterCol.Length;
        BossController bossController;
        foreach (var monster in inRangeMonsterCol)
        {
            if (monster.GetComponent<Controlble>().isDie)
            {
                count--;
            }
            if (monster.TryGetComponent<BossController>(out bossController))
            {
                if (bossController.active)
                {
                    count--;
                }
            }
        }
        if (count == 1)
            return;

        vignetteCenter = new Vector2(vignetteCenterX, vignetteCenterY);
        CameraManager.Instance.StartSetVignette(0.7f, 0.35f, 1f, vignetteCenter);
        Time.timeScale = 0f;
        closestDistance = bodySwapRange+3;

        foreach (var monster in inRangeMonsterCol)
        {
            if (monster.GetComponent<PlayerInput>().enabled == true || monster.GetComponent<Controlble>().isDie)
            {
                continue;
            }

            distance = Vector2.Distance(transform.position, monster.transform.position);

            if (closestDistance > distance)
            {
                closestDistance = distance;
                mostCloseMonster = monster;
            }
        }
        
        currentSelectUnit = mostCloseMonster.gameObject;
        currentSelectMark = currentSelectUnit.transform.Find("CurrentSelectMark").gameObject;
        currentSelectMark.SetActive(true);
        currentInputSystem.SwitchCurrentActionMap("BodySwapAction");
    }

    public void OnSelectMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        float smallestAngle;

        if(!context.performed)
            return;

        #region 인풋 방향 체크
        if (context.ReadValue<Vector2>() == Vector2.up)
        {
            viewRotateZ = 0;
        }
        else if (context.ReadValue<Vector2>() == Vector2.right)
        {
            viewRotateZ = 90f;
        }
        else if (context.ReadValue<Vector2>() == Vector2.left)
        {
            viewRotateZ = -90f;
        }
        else if (context.ReadValue<Vector2>() == Vector2.down)
        {
            viewRotateZ = 180f;
        }
        #endregion
        
        closestDistance = bodySwapRange + 3f;
        smallestAngle = viewHalfAngle;

        foreach (var monster in inRangeMonsterCol)
        {
            if (monster.GetComponent<PlayerInput>().enabled == true || monster.GetComponent<Controlble>().isDie)
            {
                continue;
            }
            
            Vector3 dir = (monster.transform.position - currentSelectUnit.transform.position).normalized;
            Vector2 lookDir = AngleToDirZ(viewRotateZ);

            float dot = Vector2.Dot(lookDir, dir);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            
            if (angle <= viewHalfAngle)
            {
                distance = Vector2.Distance(currentSelectUnit.transform.position, 
                    monster.transform.position);
                if (closestDistance > distance)
                {
                    closestDistance = distance;
                    mostCloseMonster = monster;
                }
            }
        }
        
        currentSelectMark.SetActive(false);
        currentSelectUnit = mostCloseMonster.gameObject;
        currentSelectMark = currentSelectUnit.transform.Find("CurrentSelectMark").gameObject;
        currentSelectMark.SetActive(true);
    }

    public virtual void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.started || CameraManager.Instance.closing)
            return;
        if (!isSwap)
        {
            isSwap = true;
            BossController bossController;
            if (currentSelectUnit.TryGetComponent<BossController>(out bossController))
            {
                bossController.animator.SetBool("deactive", false);
                bossController.unitCurrentHp = unitHp;
            }
            if (currentSelectUnit.CompareTag("Enemy"))
            {
                UIManager.Instance.EnablePlayerStatus(AgentDictionaly.Instance.UnitDataTypeSoDic["Player"], playerInput.GetComponent<PlayerController>());
            }
            else
                UIManager.Instance.DisablePlayerStatus();

            
            animator.SetFloat("speed", 0);
            swapInputSystem = currentSelectUnit.GetComponent<PlayerInput>();
            Controlble swapController = currentSelectUnit.GetComponent<Controlble>();;
            
            currentSelectMark.SetActive(false);
            CameraManager.Instance.CameraFollowChange(currentSelectUnit.transform);
            
            currentInputSystem.enabled = false;
            isControlling = false;
            swapInputSystem.enabled = true;
            swapController.isControlling = true;
            UIManager.Instance.StartUpdateStatus(AgentDictionaly.Instance.UnitDataTypeSoDic[swapController.unitName], swapController);
            
            CameraManager.Instance.StartSetVignette(0.25f, 0.7f, 1f, vignetteCenter);
            Time.timeScale = 1f;
            isSwap = false;
        }
    }

    public void OnCancelBodySwap(InputAction.CallbackContext context)
    {
        if (!context.started || isSwap == true || CameraManager.Instance.closing)
            return;
        CameraManager.Instance.StartSetVignette(0.25f, 0.7f, 1f, vignetteCenter);
        currentInputSystem.SwitchCurrentActionMap("PlayerAction");
        currentSelectMark.SetActive(false);
        Time.timeScale = 1f;
    }

    #region debug

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bodySwapRange);
    }
    
    private void OnDrawGizmos()
    {
        if (debugMod)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector2.down * jumpCheckDistance);
            viewHalfAngle = viewAngle * 0.5f;

            Vector3 rightDir = AngleToDirZ(-viewHalfAngle + viewRotateZ);
            Vector3 leftDir = AngleToDirZ(viewHalfAngle + viewRotateZ);
            Vector3 lookDir = AngleToDirZ(viewRotateZ);
            
            Debug.DrawRay(transform.position, leftDir * bodySwapRange, Color.cyan);
            Debug.DrawRay(transform.position, lookDir * bodySwapRange, Color.green);
            Debug.DrawRay(transform.position, rightDir * bodySwapRange, Color.cyan);
        }
    }
    

    #endregion

    private Vector3 AngleToDirZ(float angleInDegree)
    {
        float radian = (angleInDegree - transform.eulerAngles.z) * Mathf.Deg2Rad;
        return new Vector3(MathF.Sin(radian), Mathf.Cos(radian), 0);
    }
}
