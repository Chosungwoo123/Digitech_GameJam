using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    private SpriteRenderer sr;

    public void InitAfterImage(Sprite sprite, int direction)
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = sprite;
        transform.localScale = new Vector3(direction, 1, 1);

        StartCoroutine(AlphaRoutine());
    }

    private IEnumerator AlphaRoutine()
    {
        float startAmount = sr.color.a;
        float curAlpha = startAmount;
        float temp = 0;
        float time = 1f;

        while (temp <= time)
        {
            curAlpha -= Time.deltaTime / time * startAmount;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, curAlpha);

            temp += Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}