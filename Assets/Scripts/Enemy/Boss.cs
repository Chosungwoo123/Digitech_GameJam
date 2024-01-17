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

    public AudioClip hitSound;

    public float damage = 10;

    private float curHealth;

    private Animator anim;

    private void Start()
    {
        curHealth = maxHealth;
        anim = GetComponent<Animator>();

        StartCoroutine(ThinkingPattern());
    }

    private void Update()
    {
        healthImage.fillAmount = curHealth / maxHealth;
        healthText.text = curHealth + " / " + maxHealth;
    }

    private IEnumerator ThinkingPattern()
    {
        transform.DOMove(new Vector3(0, 12, 0), 0.5f);

        yield return new WaitForSeconds(Random.Range(5, 8));

        int randomPattern = Random.Range(0, 5);

        switch (randomPattern)
        {
            case 0: 
                StartCoroutine(Pattern01());
                break;
            case 1:
                StartCoroutine(Pattern02());
                break;
            case 2:
                StartCoroutine(Pattern03());
                break;
            case 3:
                StartCoroutine(Pattern04());
                break;
            case 4:
                StartCoroutine(Pattern05());
                break;
        }
    }

    private IEnumerator Pattern01()
    {
        transform.DOMove(Vector3.zero, 0.3f);

        yield return new WaitForSeconds(0.5f);

        int shootCount = 70;
        int bulletCount = 5;

        WaitForSeconds interval = new WaitForSeconds(0.1f);

        float offset = 0;

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < 360; j += 360 / bulletCount)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, j + offset)).Init(10, damage, true, 5, 6);
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, -(j + offset))).Init(10, damage, true, 5, 6);
            }

            yield return interval;

            offset += 5f;
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(ThinkingPattern());
    }

    private IEnumerator Pattern02()
    {
        transform.DOMove(new Vector3(-44, 46), 0.8f);

        yield return new WaitForSeconds(1f);

        transform.DOMove(new Vector3(42, -19), 6);

        int shootCount = 20;
        int bulletCount = 30;

        WaitForSeconds interval = new WaitForSeconds(0.3f);

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < 360; j += 360 / bulletCount)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, j)).Init(10, damage, true, 5, 6);
            }

            yield return interval;
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(ThinkingPattern());
    }

    private IEnumerator Pattern03()
    {
        transform.DOMove(new Vector3(42, 46), 0.8f);

        yield return new WaitForSeconds(1f);

        transform.DOMove(new Vector3(-44, -19), 6);

        int shootCount = 60;
        int bulletCount = 10;

        WaitForSeconds interval = new WaitForSeconds(0.1f);

        float offset = 0;

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < 360; j += 360 / bulletCount)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, j + offset)).Init(10, damage, true, 5, 6);
            }

            offset += 6f;

            yield return interval;
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(ThinkingPattern());
    }

    private IEnumerator Pattern04()
    {
        transform.DOMove(new Vector3(0, 48), 0.8f);

        yield return new WaitForSeconds(1f);

        int shootCount = 6;
        int bulletShootCount = 6;

        WaitForSeconds interval = new WaitForSeconds(0.2f);

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < bulletShootCount; j++)
            {
                SectorFromTargetShoot(7, 80);
                yield return interval;
            }

            yield return new WaitForSeconds(0.8f);
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(ThinkingPattern());
    }

    private IEnumerator Pattern05()
    {
        transform.DOMove(new Vector3(35, 21.5f), 0.8f);

        yield return new WaitForSeconds(1f);

        int shootCount = 20;
        int bulletCount = 10;

        float offset = 0;

        WaitForSeconds interval = new WaitForSeconds(0.1f);

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < 360; j += 360 / bulletCount)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, j + offset)).Init(10, damage, true, 5, 6);
            }

            offset += 6f;

            yield return interval;
        }

        yield return new WaitForSeconds(3f);

        interval = new WaitForSeconds(0.3f);
        transform.DOMove(new Vector3(-26, -2), 3);

        bulletCount = 30;
        shootCount = 10;

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < 360; j += 360 / bulletCount)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, j)).Init(15, damage, true, 5, 6);
            }

            yield return interval;
        }

        shootCount = 20;
        interval = new WaitForSeconds(0.15f);
        for (int i = 0; i < shootCount; i++)
        {
            SectorFromTargetShoot(7, 80);
            yield return interval;
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(ThinkingPattern());
    }

    private void SectorFromTargetShoot(int count, float central)
    {
        Vector2 nor = (GameManager.Instance.curPlayer.transform.position - transform.position).normalized;
        float tarZ = Mathf.Atan2(nor.y, nor.x) * Mathf.Rad2Deg;

        float amount = central / (count - 1);
        float z = central / -2f + (int)tarZ;

        for (int i = 0; i < count; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, z);
            Instantiate(bulletPrefab, transform.position, rot).Init(10, damage, true, 10, 8);
            z += amount;
        }
    }

    public void OnDamage(float damage, Vector3 bulletPos)
    {
        curHealth -= damage;

        damagePopup.Spawn((Vector2)transform.position + (Random.insideUnitCircle * Random.Range(0, 7)), damage);
        SoundManager.Instance.PlaySound(hitSound);

        if (curHealth <= 0)
        {
            // Á×´Â ·ÎÁ÷
            gameObject.SetActive(false);
            GameManager.Instance.GameClear();
            return;
        }

        anim.SetTrigger("Damage");

        Vector3 dir = transform.position - bulletPos;
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, dir.normalized);
        Instantiate(hitEffect, transform.position, rot);
    }
}