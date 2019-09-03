using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupDispenser : Machine
{
    public string size;

    public override bool Interact(PlayerInteract interactor)
    {
        base.Interact(interactor);

        if (!interacting)
            interacting = true;
        else
        {
            interacting = false;
            interactor.heldBoba = DispenseCup();
        }

        return interacting;
    }

    private Boba DispenseCup()
    {
        Boba boba = new Boba();
        boba.size = size;
        return boba; // TEST
    }

}
