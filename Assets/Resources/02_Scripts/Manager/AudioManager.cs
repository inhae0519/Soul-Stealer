using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource effectSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip audioClip)
    {
        bgmSource.clip = audioClip;
        bgmSource.Play();
    }

    public void PlayEffect(AudioClip audioClip)
    {
        effectSource.PlayOneShot(audioClip);
    }

    public void LaserSound(AudioClip audioClip)
    {
        effectSource.clip = audioClip;
        effectSource.Play();
    }

    public void LaserSoundStop()
    {
        effectSource.Stop();
    }
}