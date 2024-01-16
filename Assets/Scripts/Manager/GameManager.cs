using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 싱글톤

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
    public Image fadeImage;
    public GameObject gameOverObj;

    [Space(10)]
    [Header("아이템 변수")]
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
    [Header("아이템 UI")]
    public GameObject busObj;
    public TextMeshProUGUI busLevelText;
    public GameObject treeObj;
    public TextMeshProUGUI treeLevelText;
    public GameObject tumblerObj;
    public TextMeshProUGUI tumblerLevelText;
    public GameObject strawObj;
    public TextMeshProUGUI strawLevelText;

    [Space(10)]
    public GameObject boss;

    public AudioClip bgm;
    public AudioClip npcBgm;
    public AudioClip bossBgm;
    public AudioClip missionClearSound;
    public AudioClip itemSound;
    public AudioClip gameOverBgm;

    private int busLevel;
    private int treeLevel;
    private int tumblerLevel;
    private int strawLevel;

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

    private void Start()
    {
        SoundManager.Instance.PlayMusic(bgm);
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
        SoundManager.Instance.PlayMusic(bgm);
        SoundManager.Instance.PlaySound(missionClearSound);

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
        SoundManager.Instance.PlaySound(itemSound);

        healthMultiply = healthMultiply + 0.1f;
        curPlayer.GetComponent<PlayerMovement>().SetHealth();
    }

    public void DamageUpgrade()
    {
        treeObj.SetActive(true);
        treeLevel++;
        treeLevelText.text = "X" + treeLevel;
        SoundManager.Instance.PlaySound(itemSound);

        damageMultiply = damageMultiply + 0.1f;
    }

    public void MoveSpeedUpgrade()
    {
        busObj.SetActive(true);
        busLevel++;
        busLevelText.text = "X" + busLevel;
        SoundManager.Instance.PlaySound(itemSound);

        moveSpeedMultiply = moveSpeedMultiply + 0.1f;
    }

    public void AttactRateUpgrade()
    {
        strawObj.SetActive(true);
        strawLevel++;
        strawLevelText.text = "X" + strawLevel;
        SoundManager.Instance.PlaySound(itemSound);

        attackRateMultiply = attackRateMultiply - 0.1f;
    }

    public void SelectNPC(NPC _npc)
    {
        npc = _npc;
        SoundManager.Instance.PlayMusic(npcBgm);
    }

    private void ShowBoss()
    {
        for (int i = enemySpanwers.Length - 1; i >= 0; i--)
        {
            Destroy(enemySpanwers[i].gameObject);
        }

        SoundManager.Instance.PlayMusic(bossBgm);
        vc.m_Lens.OrthographicSize = 30;
        bg.transform.localScale = new Vector3(2f, 2f, 2f);
        boss.SetActive(true);
    }

    public void GameClear()
    {
        StartCoroutine(GameClearRoutine());
    }

    private IEnumerator GameClearRoutine()
    {
        float targetAlpha = 1;
        float curAlpha = 0;
        float temp = 0;

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, curAlpha);
        cameraShake.ShakeCamera(20, 5);

        while (temp <= 5)
        {
            curAlpha += Time.deltaTime * targetAlpha / 5;

            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, curAlpha);

            temp += Time.deltaTime;

            yield return null;
        }

        SoundManager.Instance.StopMusic();
        SceneManager.LoadScene("OutroScene");
    }

    public void GameOver()
    {
        gameOverObj.SetActive(true);
        isStop = true;
        SoundManager.Instance.PlayMusic(gameOverBgm);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}