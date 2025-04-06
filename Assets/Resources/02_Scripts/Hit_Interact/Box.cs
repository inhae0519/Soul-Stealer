using System;
using Unity.Mathematics;
using UnityEngine;

public class Box : Hit_Interactable
{
    [SerializeField] private GameObject item;
    private GameObject basebox;
    
    private void Start()
    {
        hp = 3;
        basebox = transform.Find("Box").gameObject;
    }

    public override void Interact()
    {
        DropItem();
    }

    private void DropItem()
    {
        Instantiate(item, new Vector2(transform.position.x, transform.position.y + 1), quaternion.identity);
        Destroy(transform.root.gameObject);
    }
}