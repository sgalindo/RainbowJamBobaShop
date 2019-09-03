using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaDispenser : Machine
{
    public string teaType;

    public override bool Interact(PlayerInteract interactor)
    {
        if (interactor.heldBoba == null)
            return false;
        if (interactor.heldBoba.pearls == "")
            return false;

        base.Interact(interactor);

        if (!interacting)
            interacting = true;
        else
        {
            interacting = false;
            DispenseTea(interactor.heldBoba, teaType);
        }

        return interacting;
    }

    private void DispenseTea(Boba boba, string teaType)
    {
        boba.flavor = teaType;
        boba.ready = true;
    }
}
