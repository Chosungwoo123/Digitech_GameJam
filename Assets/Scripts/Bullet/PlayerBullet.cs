using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject enemyHitEffect;
    [SerializeField] private GameObject groundHitEffect;

    public void Init(float speed, float damage)
    {
        this.damage = damage;
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    private void SpawnParticle(Vector3 spawnPoint, Vector3 dir)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, dir);
        Instantiate(enemyHitEffect, spawnPoint, rot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().OnDamage(damage);
            SpawnParticle(transform.position, transform.position - collision.transform.position);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Ground"))
        {
            Instantiate(groundHitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}