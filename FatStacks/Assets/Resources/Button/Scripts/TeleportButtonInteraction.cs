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
    public override void Interact(Pickup pickup)
    {
        pickup.character.transform.position = destinationCoordinates.position;
        pickup.character.transform.localRotation = destinationCoordinates.localRotation;
    }
}
