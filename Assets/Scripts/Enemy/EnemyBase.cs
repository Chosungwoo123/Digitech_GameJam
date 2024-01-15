using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float scanRadius;
    public float moveSpeed;

    public LayerMask playerLayer;
    public LayerMask groundLayer;

    #region Ground Check

    public Transform groundCheckPos;
    public float groundCheckRange;

    #endregion

    private float movementDirection;
    private float xScale;

    private bool isGrounded;

    private Vector3 startPos;
    private Vector3 targetPos;

    private Collider2D player;
    private Rigidbody2D rigid;

    private void Start()
    {
        startPos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        xScale = transform.localScale.x;
    }

    private void Update()
    {
        CheckingTarget();
        CheckMoveDirection();
        CheckGround();
        FlipUpdate();
        MoveUpdate();
    }

    private void FixedUpdate()
    {
        ScanPlayer();
    }

    private void CheckingTarget()
    {
        if (player != null)
        {
            targetPos = player.gameObject.transform.position;
            startPos.y = transform.position.y;
            targetPos.y = transform.position.y;
        }
        else
        {
            targetPos = startPos;
            startPos.y = transform.position.y;
            targetPos.y = transform.position.y;
        }
    }

    private void CheckMoveDirection()
    {
        Vector2 dir = targetPos - transform.position;

        if (dir.x > 0)
        {
            movementDirection = 1;
        }
        else 
        {
            movementDirection = -1;
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheckPos.position, Vector3.down, groundCheckRange, groundLayer);

        if (hit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void FlipUpdate()
    {
        if (movementDirection > 0)
        {
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        }
        else if (movementDirection < 0)
        {
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
        }
    }

    private void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.1f || !isGrounded)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(movementDirection * moveSpeed, rigid.velocity.y);
        }
    }

    private void ScanPlayer()
    {
        player = Physics2D.OverlapCircle(startPos, scanRadius, playerLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (startPos == Vector3.zero)
        {
            Gizmos.DrawWireSphere(transform.position, scanRadius);
        }
        else
        {
            Gizmos.DrawWireSphere(startPos, scanRadius);
        }

        Gizmos.DrawRay(groundCheckPos.position, Vector3.down * groundCheckRange);
    }
}