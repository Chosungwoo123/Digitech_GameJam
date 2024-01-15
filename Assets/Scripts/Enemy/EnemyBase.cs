using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float scanRadius;
    public float moveSpeed;
    public float maxHealth;

    public LayerMask playerLayer;
    public LayerMask groundLayer;

    #region Ground Check

    [Space(10)]
    [Header("Ground Check")]
    public Transform groundCheckPos;
    public float groundCheckRange;

    #endregion

    #region Weapon Info

    [Space(10)]
    [Header("Weapon Info")]
    public float fireRate;
    public float bulletDamage;
    public int bulletCount;
    public float fireInterval;
    public EnemyBullet bulletPrefab;
    public Transform shootPos;
    public float bulletSpeed;
    public Transform weaponPivot;
    public int sectorCount;
    public bool sectorAttack;

    #endregion

    public GameObject hitEffect;

    private float movementDirection;
    private float xScale;
    private float fireTimer;
    private float curHealth;
    private float bulletAngle;

    private bool isGrounded;
    private bool isTargeting;
    private bool isAttacking;
    private bool isLookRight;

    private Vector3 startPos;
    private Vector3 targetPos;

    private Collider2D player;
    private Rigidbody2D rigid;

    private WaitForSeconds fireIntervalSeconds;

    private void Start()
    {
        startPos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        xScale = transform.localScale.x;
        fireIntervalSeconds = new WaitForSeconds(fireInterval);
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (GameManager.Instance.isStop)
        {
            return;
        }

        CheckingTarget();
        CheckMoveDirection();
        CheckGround();
        FlipUpdate();
        MoveUpdate();
        AngleUpdate();
        WeaponAngleUpdate();
        AttackUpdate();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isStop)
        {
            return;
        }

        ScanPlayer();
    }

    private void CheckingTarget()
    {
        if (player != null && player.transform.position.y >= transform.position.y)
        {
            targetPos = player.gameObject.transform.position;
            startPos.y = transform.position.y;
            targetPos.y = transform.position.y;
            isTargeting = true;
        }
        else
        {
            targetPos = startPos;
            startPos.y = transform.position.y;
            targetPos.y = transform.position.y;
            isTargeting = false;
        }
    }

    private void CheckMoveDirection()
    {
        Vector2 dir = targetPos - transform.position;

        if (dir.x > 0)
        {
            movementDirection = 1;
        }
        else if(dir.x < 0)
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
            isLookRight = true;
        }
        else if (movementDirection < 0)
        {
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
            isLookRight = false;
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

    private void AngleUpdate()
    {
        Vector2 dir = GameManager.Instance.curPlayer.transform.position - transform.position;
        bulletAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void WeaponAngleUpdate()
    {
        if (isTargeting)
        {
            weaponPivot.rotation = Quaternion.AngleAxis(bulletAngle + (isLookRight ? 0 : 180), Vector3.forward);
            shootPos.localRotation = Quaternion.AngleAxis((isLookRight ? 0 : 180), Vector3.up);
        }
        else
        {
            weaponPivot.rotation = Quaternion.identity;
        }
    }

    private void AttackUpdate()
    {
        if (isAttacking || !isTargeting)
        {
            return;
        }

        if (isTargeting && fireTimer >= fireRate && !isAttacking)
        {
            StartCoroutine(ShootRoutine());
            fireTimer = 0;
        }

        fireTimer += Time.deltaTime;
    }

    private IEnumerator ShootRoutine()
    {
        isAttacking = true;

        for (int i = 0; i < bulletCount; i++)
        {
            if (sectorAttack)
            {
                SectorFromTargetShoot(sectorCount, 60);
            }
            else
            {
                var bullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.Euler(0,0,bulletAngle));
                bullet.Init(bulletSpeed, bulletDamage);
            }

            yield return fireIntervalSeconds;
        }   

        isAttacking = false;
    }

    private void SectorFromTargetShoot(int count, float central)
    {
        float amount = central / (count - 1);
        float z = central / -2f + (int)bulletAngle;

        for (int i = 0; i < count; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, z);
            var bullet = Instantiate(bulletPrefab, shootPos.position, rot);
            bullet.Init(bulletSpeed, bulletDamage);
            z += amount;
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

    public void OnDamage(float damage, Vector3 bulletPos)
    {
        curHealth -= damage;

        if (curHealth <= 0)
        {
            // Á×´Â ·ÎÁ÷
            Destroy(gameObject);
            return;
        }

        Vector3 dir = transform.position - bulletPos;
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, dir.normalized);
        Instantiate(hitEffect, transform.position, rot);
    }
}