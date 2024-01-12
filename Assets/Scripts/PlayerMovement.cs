using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rigid;

    private Movement movement;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        movement = new Movement(moveSpeed, rigid);
    }

    private void FixedUpdate()
    {
        movement.MoveUpdate();
    }
}

class Movement
{
    private float moveSpeed;

    private Vector2 moveVec;

    private Rigidbody2D rigid;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }
    public Movement(float moveSpeed, Rigidbody2D rigid)
    {
        this.moveSpeed = moveSpeed;
        this.rigid = rigid;
    }

    public void MoveUpdate()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveVec.y = rigid.velocity.y;

        rigid.velocity = moveVec;
    }
}

class JumpClass
{
    private float jumpForce;

    public float JumpForce
    {
        get
        {
            return jumpForce;
        }
        set
        {
            jumpForce = value;
        }
    }
}