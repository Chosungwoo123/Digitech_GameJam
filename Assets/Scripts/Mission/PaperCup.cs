using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaperCup : MonoBehaviour
{
    public GameObject effect;

    private TumblerMission mission;

    public void Init(TumblerMission mission)
    {
        this.mission = mission;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Debug.Log("Å¬¸¯");

        Instantiate(effect, transform.position, Quaternion.identity);
        mission.Success();
        Destroy(gameObject);
    }
}