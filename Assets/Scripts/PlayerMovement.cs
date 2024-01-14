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

    #endregion

    private int jumpCounter;

    private float movementInputDirection;
    private float angle;

    private bool isGrounded;
    private bool canJump;
    private bool isAttacking;

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
        if (movementInputDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void AngleUpdate()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}