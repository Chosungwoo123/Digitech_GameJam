using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DamageNumbersPro;
using TMPro;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    public float maxHealth;
    public EnemyBullet bulletPrefab;
    public Image healthImage;

    public DamageNumber damagePopup;
    public GameObject hitEffect;
    public TextMeshProUGUI healthText;

    private float curHealth;

    private Animator anim;

    private void Start()
    {
        curHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        healthImage.fillAmount = curHealth / maxHealth;
        healthText.text = curHealth + " / " + maxHealth;
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

    public void OnDamage(float damage, Vector3 bulletPos)
    {
        curHealth -= damage;

        damagePopup.Spawn((Vector2)transform.position + (Random.insideUnitCircle * Random.Range(0, 7)), damage);

        if (curHealth <= 0)
        {
            // Á×´Â ·ÎÁ÷
            gameObject.SetActive(false);
            return;
        }

        anim.SetTrigger("Damage");

        Vector3 dir = transform.position - bulletPos;
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, dir.normalized);
        Instantiate(hitEffect, transform.position, rot);
    }
}