using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlDispenser : Machine
{
    public string pearlType;

    public override bool Interact(PlayerInteract interactor)
    {
        if (interactor.heldBoba == null)
            return false;

        base.Interact(interactor);

        if (!interacting)
            interacting = true;
        else
        {
            interacting = false;
            DispensePearls(interactor.heldBoba, pearlType);
        }

        return interacting;
    }

    private void DispensePearls(Boba boba, string pearlType)
    {
        boba.pearls = pearlType;
        boba.ready = true;
    }
}
