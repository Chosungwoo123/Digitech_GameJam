using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumblerMission : MonoBehaviour
{
    public GameObject tumblerMission;
    public GameObject paperCupPrefab;

    public int min;
    public int max;

    private int count;
    private int counter;

    private void Start()
    {
        GameManager.Instance.ShowTumblerUI();

        count++;
        var paperCup = Instantiate(paperCupPrefab, transform);
        paperCup.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        paperCup.transform.localPosition = new Vector3(Random.Range(-11, 12), Random.Range(-5, 6), -2);
        paperCup.GetComponent<PaperCup>().Init(this);

        int temp = Random.Range(min, max);

        for (int i = 0; i < temp; i++)
        {
            int j = Random.Range(0, 2);

            if (j == 0)
            {
                var item = Instantiate(tumblerMission, transform);
                item.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                item.transform.localPosition = new Vector2(Random.Range(-11, 12), Random.Range(-5, 6));
            }
            else if (j == 1)
            {
                var item = Instantiate(paperCupPrefab, transform);
                item.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                item.transform.localPosition = new Vector3(Random.Range(-11, 12), Random.Range(-5, 6), -2);
                item.GetComponent<PaperCup>().Init(this);
                count++;
            }
        }
    }

    public void Success()
    {
        counter++;

        if(counter == count)
        {
            Debug.Log("Å¬¸®¾î");
            GameManager.Instance.MissionClear();
            Destroy(gameObject);
        }
    }
}