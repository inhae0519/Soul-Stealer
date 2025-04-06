using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractObj : MonoBehaviour
{
    public void ObjTrigger()
    {
        ObjEffect();
    }

    public virtual void ObjEffect()
    {
        
    }
}
