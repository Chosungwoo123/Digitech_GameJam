using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject[] missionPrefabs;

    public GameObject[] itemPrefabs;

    private bool isEnter;

    private void Update()
    {
        if (GameManager.Instance.isStop)
        {
            return;
        }

        if (isEnter && Input.GetKeyDown(KeyCode.F))
        {
            var mission = Instantiate(missionPrefabs[Random.Range(0, missionPrefabs.Length)], GameManager.Instance.mainCamera.transform.position, Quaternion.identity);
            mission.transform.position = new Vector3(mission.transform.position.x, mission.transform.position.y, 0);
            GameManager.Instance.SelectNPC(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnter = false;
        }
    }

    public void GiveItem()
    {
        Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}