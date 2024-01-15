using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusItem : MonoBehaviour
{
    public GameObject infoObj;

    private bool isEnter;

    private void Start()
    {
        infoObj.SetActive(false);
    }

    private void Update()
    {
        if (isEnter && Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.MoveSpeedUpgrade();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            infoObj.SetActive(true);
            isEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            infoObj.SetActive(false);
            isEnter = false;
        }
    }
}