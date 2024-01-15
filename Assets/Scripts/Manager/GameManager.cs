using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    #endregion

    public Camera mainCamera;
    public GameObject curPlayer;
    public bool isStop;

    public GameObject SWCUiObj;
    public GameObject savePowerUiObj;
    public ParticleSystem missionClearEffect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowSWCUI()
    {
        isStop = true;
        SWCUiObj.SetActive(true);
    }

    public void ShowSavePowerUI()
    {
        isStop = true;
        savePowerUiObj.SetActive(true);
    }

    public void MissionClear()
    {
        isStop = false;
        SWCUiObj.SetActive(false);
        savePowerUiObj.SetActive(false);

        missionClearEffect.Play();
    }
}