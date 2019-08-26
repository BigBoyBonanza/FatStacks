using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotInteraction : Interaction
{
    public Interaction parrot;

    public override string GetPrompt(int index = -1)
    {
        return parrot.GetPrompt();
    }

    public override void Interact(Pickup obj)
    {
        parrot.Interact(obj);
    }

    public override bool IsBusy()
    {
        isBusy = parrot.IsBusy();
        return isBusy;
    }
}
