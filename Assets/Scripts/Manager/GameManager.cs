using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject tumblerUiObj;
    public ParticleSystem missionClearEffect;
    public TextMeshProUGUI healthText;

    [Space(10)]
    [Header("æ∆¿Ã≈€ ∫Øºˆ")]
    public float damageMultiply = 1;
    public float moveSpeedMultiply = 1;
    public float attackRateMultiply = 1;
    public float healthMultiply = 1;

    private NPC npc;

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

    private void Update()
    {
        healthText.text = curPlayer.GetComponent<PlayerMovement>().curHealth + " / " + curPlayer.GetComponent<PlayerMovement>().maxHealth;
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

    public void ShowTumblerUI()
    {
        isStop = true;
        tumblerUiObj.SetActive(true);
    }

    public void MissionClear()
    {
        isStop = false;
        SWCUiObj.SetActive(false);
        savePowerUiObj.SetActive(false);
        tumblerUiObj.SetActive(false);
        npc.GiveItem();

        missionClearEffect.Play();
    }

    public void HealthUpgrade()
    {
        healthMultiply = healthMultiply + 0.1f;
        curPlayer.GetComponent<PlayerMovement>().SetHealth();
    }

    public void DamageUpgrade()
    {
        damageMultiply = damageMultiply + 0.1f;
    }

    public void MoveSpeedUpgrade()
    {
        moveSpeedMultiply = moveSpeedMultiply + 0.1f;
    }

    public void AttactRateUpgrade()
    {
        attackRateMultiply = attackRateMultiply - 0.1f;
    }

    public void SelectNPC(NPC _npc)
    {
        npc = _npc;
    }
}