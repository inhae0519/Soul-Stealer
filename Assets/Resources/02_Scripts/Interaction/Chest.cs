using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] private AudioClip locksound;
    [SerializeField] private AudioClip opensound;
    [SerializeField] private AudioClip gameClip;
    private PlayerController playerController;
    [SerializeField] private GameObject item;
    [SerializeField] private bool needKey;
    private bool opened;
    private GameObject closeChest;
    private GameObject openChest;
    private Transform spawnPoint;

    private void Awake()
    {
        AudioManager.Instance.PlayBGM(gameClip);
        spawnPoint = transform.Find("SpawnPoint");
        closeChest = transform.Find("Close").gameObject;
        openChest = transform.Find("Open").gameObject;
    }

    public override void Interact()
    {
        if (opened)
            return;
        if (needKey)
        {
            if (!playerController.haveKey)
            {
                AudioManager.Instance.PlayEffect(locksound);
                return;
            }
            else
                playerController.haveKey = false;
        }
        AudioManager.Instance.PlayEffect(opensound);
        opened = true;
        UIManager.Instance.KeyImageSetActive(playerController.haveKey);
        GameObject key = Instantiate(item, spawnPoint.position, Quaternion.identity);
        closeChest.SetActive(false);
        openChest.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            playerController = other.GetComponent<PlayerController>();
    }
}
