using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public void BaseInteract()
    {
        Interact();
    }

    public virtual void Interact()
    {
        
    }
}
