using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    private void Awake()
    {
        AudioManager.Instance.PlayBGM(audioClip);
    }

    public void NextScene()
    {
        UIManager.Instance.SetStart();
        CameraManager.Instance.StartCamOff();
        UserSceneManager.Instance.NextScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
