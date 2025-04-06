using System;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        AudioManager.Instance.PlayBGM(null);
        AudioManager.Instance.PlayEffect(audioClip);
    }

    private void Start()
    {
        UIManager.Instance.EndingPanelOn();
    }
}
