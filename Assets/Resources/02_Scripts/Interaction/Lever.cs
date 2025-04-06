using System;
using UnityEngine;

public class Lever : Interactable
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject targetObject;
    private bool used;
    private GameObject useLever;
    private GameObject defaultLever;

    private void Awake()
    {
        useLever = transform.Find("Used").gameObject;
        defaultLever = transform.Find("Default").gameObject;
    }

    public override void Interact()
    {
        if (used)
            return;
        AudioManager.Instance.PlayEffect(audioClip);
        used = true;
        defaultLever.SetActive(false);
        useLever.SetActive(true);
        targetObject.GetComponent<InteractObj>().ObjTrigger();
    }
}
