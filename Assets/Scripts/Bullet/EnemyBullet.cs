using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float damage;
    public GameObject effect;
    private bool isPenetrate;
    private int penatrateCount;

    public void Init(float speed, float damage, bool penetrate = false, float lifeTime = 1f, int penatrateCount = 1)
    {
        this.damage = damage;
        isPenetrate = penetrate;
        this.penatrateCount = penatrateCount;
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;

        if (isPenetrate)
        {
            StartCoroutine(LifeRoutine(lifeTime));
        }
    }
    
    private IEnumerator LifeRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            collision.GetComponent<PlayerMovement>().OnDamage(damage);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Ground"))
        {
            penatrateCount--;
            if (penatrateCount <= 0)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}