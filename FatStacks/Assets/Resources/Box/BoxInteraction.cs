using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteraction : Interaction
{
    public override void Interact(Pickup pickup)
    {
        if (Physics.CheckBox(transform.position + new Vector3(0.5f, 1.26f, 0.5f), new Vector3(0.25f, 0.25f, 0.25f), Quaternion.identity, LayerMask.GetMask("InteractSolid")))
        {
            pickup.exception.FlashText(GetException(1), 3);
        }
        else
        {
            GetComponent<Box>().Frozen = false;
            //pickup.PickupObject(gameObject);
        }
    }
}
