using UnityEngine;

public class SideDoor : InteractObj
{
    private BoxCollider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<BoxCollider2D>();
    }

    public override void ObjEffect()
    {
        collider2D.enabled = false;
    }
}
