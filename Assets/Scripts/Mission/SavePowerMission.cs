using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePowerMission : MonoBehaviour
{
    public int itemCount;

    private int itemCounter;

    private void Start()
    {
        GameManager.Instance.ShowSavePowerUI();
        GameManager.Instance.isStop = true;
    }

    public void Success()
    {
        itemCounter++;

        if (itemCounter == itemCount)
        {
            Debug.Log("Å¬¸®¾î");
            GameManager.Instance.MissionClear();
            Destroy(gameObject);
        }
    }

    public void Minus()
    {
        itemCounter--;
    }
}