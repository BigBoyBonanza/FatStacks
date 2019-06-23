using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportButtonInteraction : Interaction {

    public Transform destinationCoordinates;
    public string destinationName;

    private void Start()
    {
        _PromptData.prompts[0] = "TELEPORT " + destinationName;
    }
    public override void interact(Pickup pickup)
    {
        pickup.Character.transform.position = destinationCoordinates.position;
        pickup.Character.transform.localRotation = destinationCoordinates.localRotation;
    }
}
