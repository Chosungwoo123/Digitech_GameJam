using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float damage;

    public void Init(float speed, float damage)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}