using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject[] missionPrefabs;

    private bool isEnter;

    private void Update()
    {
        if (isEnter && Input.GetKeyDown(KeyCode.F))
        {
            var mission = Instantiate(missionPrefabs[Random.Range(0, missionPrefabs.Length)], GameManager.Instance.mainCamera.transform.position, Quaternion.identity);
            mission.transform.position = new Vector3(mission.transform.position.x, mission.transform.position.y, 0);
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
}