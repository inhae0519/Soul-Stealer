using System;
using System.Collections;
using UnityEngine;

public abstract class Hit_Interactable : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Material hitMat;
    protected Material defaultMat;
    protected int hp;

    protected void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        hitMat = Resources.Load<Material>("07_Mat/HitMat");
        defaultMat = spriteRenderer.material;
    }

    public void BaseInteract()
    {
        Interact();
    }

    public virtual void Interact()
    {
        
    }

    public void TakeDamage()
    {
        hp--;
        Debug.Log("hit");
        StartCoroutine(HitEffect());
        if (hp < 0)
            Die();
    }

    private IEnumerator HitEffect()
    {
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMat;
    }

    private void Die()
    {
        BaseInteract();
    }
}
