using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWCMission : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public SWCItem[] items;

    public int minCount;
    public int maxCount;

    private int itemCount;
    private int successCount;

    private void Start()
    {
        itemCount = Random.Range(minCount, maxCount);

        GameManager.Instance.ShowSWCUI();

        for (int i = 0; i < itemCount; i++)
        {
            var item = Instantiate(items[Random.Range(0, items.Length)], 
                spawnPoints[Random.Range(0, spawnPoints.Length)].position, 
                Quaternion.Euler(0, 0, Random.Range(0, 360)));

            item.Init(this);
        }
    }

    private void Update()
    {
        transform.position = new Vector3(GameManager.Instance.mainCamera.transform.position.x, GameManager.Instance.mainCamera.transform.position.y, 0);
    }

    public void Success()
    {
        successCount++;

        if (successCount == itemCount)
        {
            Debug.Log("Å¬¸®¾î");
        }
    }

    public void Minus()
    {
        successCount--;
    }
}