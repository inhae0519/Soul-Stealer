using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserSceneManager : MonoSingleton<UserSceneManager>
{
    private Transform playerTrm;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FindPlayerTrm;
        CameraManager.Instance.FindPlayerCam();
        CameraManager.Instance.CameraFollowChange(playerTrm);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= FindPlayerTrm;
    }

    private void FindPlayerTrm(Scene arg0, LoadSceneMode arg1)
    {
        playerTrm = GameObject.FindWithTag("Player").transform;
    }

    public void NextScene(int sceneNumber)
    {
        UIManager.Instance.EnemyStatusDestroy();
        SceneManager.LoadScene(sceneNumber);
    }
}