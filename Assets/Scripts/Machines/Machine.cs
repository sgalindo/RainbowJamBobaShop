using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    protected bool interacting;

    protected void Awake()
    {
        interacting = false;
    }

    public virtual bool Interact(PlayerInteract interactor)
    {
        Debug.Log("Interacted with " + gameObject.name);
        return true;
    }
}
