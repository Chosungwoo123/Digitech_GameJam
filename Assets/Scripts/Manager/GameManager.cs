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
}