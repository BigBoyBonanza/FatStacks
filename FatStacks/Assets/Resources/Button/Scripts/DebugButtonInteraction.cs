using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtonInteraction : Interaction {

    public override void interact(Pickup pickup)
    {
        Debug.Log("Button Pressed");
        pickup.exception.FlashText(get_exception(0), 5f);
    }
}
