using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlDispenser : Machine
{

    public override bool Interact(PlayerInteract interactor)
    {
        if (interactor.heldBoba == null)
            return false;

        base.Interact(interactor);

        if (!interacting)
        { 
            DispensePearls(interactor.heldBoba);
        }

        return false;
    }

    private void DispensePearls(Boba boba)
    {
        boba.state = Boba.State.Pearls;
    }
}
