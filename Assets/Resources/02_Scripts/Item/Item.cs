using System;
using UnityEngine;

public abstract class Item : Interactable
{
    private GameObject player;
    private bool onPlayer;
    public abstract void ItemEffect(GameObject player);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayer = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onPlayer = false;
        }
    }

    public override void Interact()
    {
        if(!onPlayer)
            return;
        ItemEffect(player);
    }
}