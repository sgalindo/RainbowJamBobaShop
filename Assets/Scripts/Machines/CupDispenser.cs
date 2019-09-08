using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupDispenser : Machine
{

    public override bool Interact(PlayerInteract interactor)
    {
        base.Interact(interactor);

        if (!interacting && interactor.heldBoba == null)
        { 
            interactor.heldBoba = DispenseCup();
        }

        return false;
    }

    private Boba DispenseCup()
    {
        Boba boba = new Boba();
        return boba;
    }

}
