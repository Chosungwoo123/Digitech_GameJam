using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    #region �̱���

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
    [Header("������ ����")]
    public float damageMultiply = 1;
    public float moveSpeedMultiply = 1;
    public float attackRateMultiply = 1;
    public float healthMultiply = 1;

    private NPC npc;

    public CameraShake cameraShake;

    public int missionCount;

    private int missionCounter;

    public EnemySpanwer[] enemySpanwers;

    public CinemachineVirtualCamera vc;
    public GameObject bg;

    [Space(10)]
    [Header("������ UI")]
    public GameObject busObj;
    public TextMeshProUGUI busLevelText;
    public GameObject treeObj;
    public TextMeshProUGUI treeLevelText;
    public GameObject tumblerObj;
    public TextMeshProUGUI tumblerLevelText;

    private int busLevel;
    private int treeLevel;
    private int tumblerLevel;

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
        missionCounter++;

        if (missionCounter >= missionCount)
        {
            ShowBoss();
        }

        missionClearEffect.Play();
    }

    public void HealthUpgrade()
    {
        tumblerObj.SetActive(true);
        tumblerLevel++;
        tumblerLevelText.text = "X" + tumblerLevel;

        healthMultiply = healthMultiply + 0.1f;
        curPlayer.GetComponent<PlayerMovement>().SetHealth();
    }

    public void DamageUpgrade()
    {
        treeObj.SetActive(true);
        treeLevel++;
        treeLevelText.text = "X" + treeLevel;

        damageMultiply = damageMultiply + 0.1f;
    }

    public void MoveSpeedUpgrade()
    {
        busObj.SetActive(true);
        busLevel++;
        busLevelText.text = "X" + busLevel;

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

    private void ShowBoss()
    {
        Debug.Log("����");

        for (int i = enemySpanwers.Length - 1; i >= 0; i--)
        {
            Destroy(enemySpanwers[i].gameObject);
        }

        vc.m_Lens.OrthographicSize = 30;
        bg.transform.localScale = new Vector3(2f, 2f, 2f);
    }
}