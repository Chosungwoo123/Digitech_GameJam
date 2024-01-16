using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI[] texts;

    private bool isEnd = false;

    private void Start()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
        }

        StartCoroutine(IntroRoutine());
    }

    private void Update()
    {
        if (Input.anyKeyDown && isEnd)
        {
            SceneManager.LoadScene("MainGame");
        }
    }

    private IEnumerator IntroRoutine()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            yield return FadeOutObject(texts[i], 0.5f);
        }

        isEnd = true;
    }

    private IEnumerator FadeOutObject(TextMeshProUGUI _text, float time)
    {
        if (time == 0)
        {
            yield break;
        }

        float targetAlpha = _text.color.a;
        float curAlpha = 0;
        float temp = 0;

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, curAlpha);

        _text.gameObject.SetActive(true);

        while (temp <= time)
        {
            curAlpha += Time.deltaTime * targetAlpha / time;

            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, curAlpha);

            temp += Time.deltaTime;

            yield return null;
        }
    }
}