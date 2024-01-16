using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxHealth;
    public EnemyBullet bulletPrefab;

    private float curHealth;

    private void Start()
    {
        curHealth = maxHealth;

        StartCoroutine(Pattern01());
    }

    private IEnumerator Pattern01()
    {
        int shootCount = 100;
        int bulletCount = 4;

        WaitForSeconds interval = new WaitForSeconds(0.1f);

        float offset = 0;

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < 360; j += 360 / bulletCount)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, j + offset)).Init(10, 10, true, 5, 4);
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, -(j + offset))).Init(10, 10, true, 5, 4);
            }

            yield return interval;

            offset += 5f;
        }
    }

    public void OnDamage(float damage)
    {
        curHealth -= damage;
    }
}