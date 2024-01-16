using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce = 16;
    public float maxHealth;
    public float curHealth;

    #region Ground Check

    [Space(10)]
    [Header("Ground Check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private int jumpCount;

    #endregion

    #region Weapon Info

    [Space(10)]
    [Header("Weapon Info")]
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private Transform shootPos;
    [SerializeField] private PlayerBullet bulletPrefab;

    #endregion

    #region Attack Info

    [Space(10)]
    [Header("Attack Info")]
    [SerializeField] private float attackRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDamage;

    #endregion

    #region Dash Info

    [Space(10)]
    [Header("Dash Info")]
    public float dashTime;
    public KeyCode dashKey = KeyCode.Mouse1;
    public float dashCoolTime;
    public float dashSpeed;
    public float afterImageDistance;
    public AfterImage afterImagePrefab;

    #endregion

    public GameObject jumpEffect;

    public float invincibilityTime;

    public AudioClip weaponSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;

    private int jumpCounter;

    private float movementInputDirection;
    private float angle;
    private float attackTimer;
    
    private float dashTimer;
    private float baseMaxHealth;

    private bool isGrounded;
    private bool canJump;
    private bool isAttacking;
    private bool isLookRight;
    private bool isDash;
    private bool canDash;
    private bool isWalk;
    private bool isInvincibility;

    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private Animator anim;

    private Vector2 mousePos;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        jumpCounter = jumpCount;
        baseMaxHealth = maxHealth;
        curHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.isStop)
        {
            rigid.velocity = Vector3.zero;
            return;
        }

        CheckInput();
        CheckSurroundings();
        CheckIfCanJump();
        CheckFlip();
        AngleUpdate();
        WeaponAming();
        AttackUpdate();
        DashUpdate();
        AnimationUpdate();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isStop)
        {
            rigid.velocity = Vector3.zero;
            return;
        }

        MoveUpdate();
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (movementInputDirection != 0 && !isDash)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetMouseButton(0))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (Input.GetKeyDown(dashKey) && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if (jumpCounter > 0)
        {
            canJump = true;
        }
        else if (isGrounded && rigid.velocity.y == 0)
        {
            canJump = true;
            jumpCounter = jumpCount;
        }
        else
        {
            canJump = false;
        }
    }

    private void CheckFlip()
    {
        if (isAttacking)
        {
            if (Mathf.Abs(angle) < 90)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isLookRight = true;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isLookRight = false;
            }

            return;
        }

        if (movementInputDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isLookRight = true;
        }
        else if (movementInputDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isLookRight = false;
        }
    }

    private void AngleUpdate()
    {
        mousePos = GameManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
    }

    private void WeaponAming()
    {
        if (isAttacking)
        {
            weaponPivot.rotation = Quaternion.AngleAxis(angle + (isLookRight ? 0 : 180), Vector3.forward);
            shootPos.localRotation = Quaternion.AngleAxis((isLookRight ? 0 : 180), Vector3.up);
        }
        else
        {
            weaponPivot.rotation = Quaternion.identity;
        }
    }

    private void AttackUpdate()
    {
        if (isAttacking && attackTimer > (attackRate * GameManager.Instance.attackRateMultiply))
        {
            Shooting();

            attackTimer = 0;
        }

        attackTimer += Time.deltaTime;
    }

    private void DashUpdate()
    {
        if (isDash)
        {
            return;
        }

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0)
        {
            canDash = true;
        }
    }

    private void AnimationUpdate()
    {
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVel", rigid.velocity.y);
    }

    private void MoveUpdate()
    {
        if (isDash)
        {
            return;
        }

        rigid.velocity = new Vector2((moveSpeed * GameManager.Instance.moveSpeedMultiply) * movementInputDirection, rigid.velocity.y);
    }

    private void Jump()
    {
        if (canJump)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            jumpCounter--;
            SoundManager.Instance.PlaySound(jumpSound);
        }
    }

    private void Shooting()
    {
        var bullet = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        bullet.Init(bulletSpeed, bulletDamage * GameManager.Instance.damageMultiply);
        SoundManager.Instance.PlaySound(weaponSound);
    }

    private IEnumerator DashRoutine()
    {
        GameManager.Instance.cameraShake.ShakeCamera(20, dashTime);
        SoundManager.Instance.PlaySound(dashSound);

        float timer = 0;

        isDash = true;
        canDash = false;

        Vector3 lastAfterImagePos = transform.position;

        Instantiate(afterImagePrefab, lastAfterImagePos, Quaternion.identity).InitAfterImage(sr.sprite, (isLookRight ? 1 : -1));

        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (movementInputDirection == 0)
        {
            movementInputDirection = (isLookRight ? 1 : -1);
        }

        rigid.velocity = new Vector2(movementInputDirection * dashSpeed, 0);

        while (timer <= dashTime)
        {
            if (GameManager.Instance.isStop)
            {
                rigid.velocity = Vector3.zero;
                isDash = false;
                yield break;
            }

            if (Vector3.Distance(transform.position, lastAfterImagePos) >= afterImageDistance)
            {
                lastAfterImagePos = transform.position;

                var afterImage = Instantiate(afterImagePrefab, lastAfterImagePos, Quaternion.identity);
                afterImage.InitAfterImage(sr.sprite, (isLookRight ? 1 : -1));
            }

            timer += Time.deltaTime;

            rigid.velocity = new Vector2(rigid.velocity.x, 0);

            yield return null;
        }

        isDash = false;

        dashTimer = dashCoolTime;

        rigid.velocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void OnDamage(float damage)
    {
        if (isDash || GameManager.Instance.isStop || isInvincibility)
        {
            return;
        }

        curHealth = Mathf.Max(0, curHealth - damage);

        if (curHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }

        GameManager.Instance.cameraShake.ShakeCamera(20, 0.3f);

        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincibility = true;
        sr.color = new Vector4(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(invincibilityTime);

        isInvincibility = false;
        sr.color = new Vector4(1, 1, 1, 1);
    }

    public void SetHealth()
    {
        maxHealth = baseMaxHealth * GameManager.Instance.healthMultiply;
        maxHealth = Mathf.Round(maxHealth);
    }

    public void HealHealth(float heal)
    {
        curHealth = Mathf.Min(maxHealth, curHealth + heal);
    }
}