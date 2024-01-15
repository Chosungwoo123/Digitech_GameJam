using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    public EnemyBase enemy;

    public float respawnTime;

    private WaitForSeconds respawn;

    private void Start()
    {
        enemy.dieEvent += DieEvent;
        respawn = new WaitForSeconds(respawnTime);
    }

    private void DieEvent()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return respawn;

        enemy.Init();
        enemy.gameObject.SetActive(true);
    }
}