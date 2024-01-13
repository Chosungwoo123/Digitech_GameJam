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

    private int jumpCounter;

    private float movementInputDirection;

    private bool isGrounded;
    private bool canJump;

    private Rigidbody2D rigid;

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
        else if (isGrounded)
        {
            canJump = true;
            jumpCounter = jumpCount;
        }
        else
        {
            canJump = false;
        }
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