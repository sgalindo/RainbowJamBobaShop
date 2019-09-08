using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaDispenser : Machine
{

    public override bool Interact(PlayerInteract interactor)
    {
        if (interactor.heldBoba == null)
            return false;
        if (interactor.heldBoba.state != Boba.State.Pearls)
            return false;

        base.Interact(interactor);

        if (!interacting)
        { 
            DispenseTea(interactor.heldBoba);
        }

        return false;
    }

    private void DispenseTea(Boba boba)
    {
        boba.state = Boba.State.Ready;
    }
}
