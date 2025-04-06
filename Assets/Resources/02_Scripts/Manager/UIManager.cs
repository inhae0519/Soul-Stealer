using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class UIManager : MonoSingleton<UIManager>
{
    private Status status;
    private GameObject statusObj;
    private GameObject playerStatusObj;
    private GameObject bossStatusObj;
    private GameObject pausePanel;
    private GameObject endingPanel;
    private GameObject gameOverPanel;
    private GameObject clearCountPanel;
    private PlayerStatus playerStatus;
    private BossStatus bossStatus;
    private GameObject enemyParent;
    [SerializeField] private float paddingX;
    private bool panelOn;
    private TextMeshProUGUI countText;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        enemyParent = transform.Find("EnemyStatusParent").gameObject;
        clearCountPanel = transform.Find("ClearCountPanel").gameObject;
        countText = transform.Find("ClearCountPanel").Find("Count").GetComponent<TextMeshProUGUI>();
        gameOverPanel = transform.Find("GameOverPanel").gameObject;
        statusObj = transform.Find("Status").gameObject;
        pausePanel = transform.Find("PausePanel").gameObject;
        endingPanel = transform.Find("EndingPanel").gameObject;
        status = transform.Find("Status").GetComponent<Status>();
        bossStatus = transform.Find("BossStatus").GetComponent<BossStatus>();
        playerStatusObj = transform.Find("PlayerStatus").gameObject;
        bossStatusObj = transform.Find("BossStatus").gameObject;
        playerStatus = playerStatusObj.GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "StartScene")
        {
            if (panelOn)
            {
                panelOn = false;
                Time.timeScale = 1;
                pausePanel.SetActive(false);
            }
            else
            {
                panelOn = true;
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void StartUpdateStatus(UnitDataTypeSO unitDataTypeSo, Controlble currentUnit)
    {
        status.UpdateStatus(unitDataTypeSo, currentUnit);
    }

    public void EnablePlayerStatus(UnitDataTypeSO unitDataTypeSo, PlayerController currentUnit)
    {
        playerStatusObj.SetActive(true);
        playerStatus.UpdateStatus(unitDataTypeSo, currentUnit);
    }

    public void DisablePlayerStatus()
    {
        playerStatusObj.SetActive(false);
    }

    public void KeyImageSetActive(bool haveKey)
    {
        status.UpdateKeyImage(haveKey);
    }

    public void EnemyStatusFollowStart(EnemyStatus enemyStatus, Vector3 pos, float paddingY)
    {
        enemyStatus.EnemyStatusFollow(pos, paddingY);
    }

    public void EnemyStatusUpdateStart(EnemyStatus enemyStatus, EnemyController enemyController)
    {
        enemyStatus.UpdateEnemyStatus(enemyController);
    }

    public void EnemyStatusEnable(GameObject enemyStatusObj)
    {
        enemyStatusObj.SetActive(true);
    }
    
    public void EnemyStatusDisable(GameObject enemyStatusObj)
    {
        enemyStatusObj.SetActive(false);
    }

    public void EnemyStatusDestroy()
    {
        foreach (var eComponent in transform.Find("EnemyStatusParent").GetComponentsInChildren<EnemyStatus>())
        {
            Destroy(eComponent.gameObject);
        }
    }

    public void BossStatusUpdateStart(BossController bossController)
    {
        bossStatus.UpdateBossStatus(bossController);
    }

    public void EnableBossStatus()
    {
        bossStatusObj.SetActive(true);
    }
    
    public void DisableBossStatus()
    {
        bossStatusObj.SetActive(false);
    }

    public void RemuseButton()
    {
        panelOn = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void TitleButton()
    {
        GameManager.Instance.StatDefault();
        UserSceneManager.Instance.NextScene(0);
        SetEnd();
        CameraManager.Instance.StartCamOn();
        endingPanel.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void EndingPanelOn()
    {
        FileIOManager.Instance.Save();
        endingPanel.SetActive(true);
    }

    public void SetStart()
    {
        statusObj.SetActive(true);
        enemyParent.SetActive(true);
    }

    public void SetEnd()
    {
        statusObj.SetActive(false);
        enemyParent.SetActive(false);
    }

    public void GameOverPanelOn()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void ReStart()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClearCountOn()
    {
        clearCountPanel.SetActive(true);
        countText.text = FileIOManager.Instance.Clear.ToString();
    }

    public void ClearCountOff()
    {
        clearCountPanel.SetActive(false);
    }
}
