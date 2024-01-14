using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce = 16;

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

    private int jumpCounter;

    private float movementInputDirection;
    private float angle;
    private float attackTimer;

    private bool isGrounded;
    private bool canJump;
    private bool isAttacking;
    private bool isLookRight;

    private Rigidbody2D rigid;

    private Vector2 mousePos;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        jumpCounter = jumpCount;
    }

    private void Update()
    {
        CheckInput();
        CheckSurroundings();
        CheckIfCanJump();
        CheckFlip();
        AngleUpdate();
        WeaponAming();
        AttackUpdate();
    }

    private void FixedUpdate()
    {
        MoveUpdate();
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

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
        if (isAttacking && attackTimer > attackRate)
        {
            Shooting();

            attackTimer = 0;
        }

        attackTimer += Time.deltaTime;
    }

    private void MoveUpdate()
    {
        rigid.velocity = new Vector2(moveSpeed * movementInputDirection, rigid.velocity.y);
    }

    private void Jump()
    {
        if (canJump)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            jumpCounter--;
        }
    }

    private void Shooting()
    {
        var bullet = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        bullet.Init(bulletSpeed, bulletDamage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}