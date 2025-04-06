using System;
using Cinemachine;
using UnityEngine;

public class PlayerEnterCheck : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Collider2D doorCol;
    [SerializeField] private GameObject bossCam;
    [SerializeField] private GameObject boss;

    private void Awake()
    {
        AudioManager.Instance.PlayBGM(audioClip);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorCol.enabled = true;
            bossCam.SetActive(true);
            UIManager.Instance.EnableBossStatus();
            boss.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
