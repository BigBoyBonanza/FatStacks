using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtonInteraction : Interaction {

    public override void Interact(Pickup pickup)
    {
        Debug.Log("Button Pressed");
        pickup.exception.FlashText(GetException(0), 5f);
    }
}
