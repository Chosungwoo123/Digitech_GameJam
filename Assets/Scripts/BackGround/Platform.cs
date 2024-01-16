using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D platformEffector;

    private bool isEnter;

    private void Awake()
    {
        gameObject.GetComponent<BoxCollider2D>().size =
            new Vector2(gameObject.GetComponent<SpriteRenderer>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y);
    }

    private void Start()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.isStop)
        {
            return;
        }

        if (isEnter && Input.GetKeyDown(KeyCode.S))
        {
            platformEffector.rotationalOffset = 180;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isEnter = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            platformEffector.rotationalOffset = 0;
            isEnter = false;
        }
    }
}