using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CameraManager : MonoSingleton<CameraManager>
{
    public bool closing;
    private Volume volume;
    private Vignette vignette;
    private ShadowsMidtonesHighlights shadowsMidtonesHighlights;
    private CinemachineVirtualCamera playerCam;
    private GameObject startCam;
    private GameObject playerCamObj;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        FindPlayerCam();
        startCam = transform.Find("StartCam").gameObject;
        playerCamObj = transform.Find("PlayerCam").gameObject;
        volume = GetComponentInChildren<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out shadowsMidtonesHighlights);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += (arg0, mode) => transform.position = Vector2.zero;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= (arg0, mode) => transform.position = Vector2.zero;
    }

    private IEnumerator OnDoVignette(float endValue, float smoothnessValue, float closeTime, Vector2 center)
    {
        closing = true;
        float currentTime = 0;
        float percent = 0;

        vignette.center.value = center;

        while (percent < 1)
        {
            currentTime += Time.unscaledDeltaTime;
            percent = currentTime / closeTime;

            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, endValue, percent);
            vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, smoothnessValue, percent);
            yield return null;
        }

        closing = false;
    }

    public void StartSetVignette(float endValue, float smoothnessValue, float closeTime, Vector2 center)
    {
        StartCoroutine(OnDoVignette(endValue, smoothnessValue , closeTime, center));
    }

    public void OnShadowMid()
    {
        shadowsMidtonesHighlights.active = true;
    }

    public void CameraFollowChange(Transform target)
    {
        playerCam.Follow = target;
    }

    public void FindPlayerCam()
    {
        playerCam = transform.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
    }

    public void StartCamOn()
    {
        playerCamObj.SetActive(false);
        startCam.SetActive(true);
    }

    public void StartCamOff()
    {
        startCam.SetActive(false);
        playerCamObj.SetActive(true);
    }
}
