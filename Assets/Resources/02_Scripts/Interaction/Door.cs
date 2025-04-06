using System;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private AudioClip locksound;
    [SerializeField] private AudioClip opensound;
    [SerializeField] private int nextSceneNumber;
    [SerializeField] private bool needKey;
    private bool opened;
    private PlayerController playerController;
    private int count;
    private GameObject openDoor;
    private GameObject closeDoor;

    private void Awake()
    {
        closeDoor = transform.Find("Close").gameObject;
        openDoor = transform.Find("Open").gameObject;
    }

    public override void Interact()
    {
        if (needKey)
        {
            if (!playerController.haveKey && !opened)
            {
                AudioManager.Instance.PlayEffect(locksound);
                return;
            }

            else
            {
                playerController.haveKey = false;
                UIManager.Instance.KeyImageSetActive(playerController.haveKey);
            }
        }
        count++;
        if (count == 1)
        {
            AudioManager.Instance.PlayEffect(opensound);
            Open();
        }
        else if (count >= 2)
        {
            CameraManager.Instance.OnShadowMid();
            GameManager.Instance.GetBackData(playerController.unitSoulLevel, playerController.unitCurrentHp, playerController.unitHp, playerController.unitAtk);
            UserSceneManager.Instance.NextScene(nextSceneNumber);
        }

    }

    private void Open()
    {
        opened = true;
        closeDoor.SetActive(false);
        openDoor.SetActive(true);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            playerController = other.GetComponent<PlayerController>();
    }
}