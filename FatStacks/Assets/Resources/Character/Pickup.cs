﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Player character;
    public ArsenalSystem gunController;
    public enum PickupState
    {
        noObjectTargeted,
        pickupObjectTargeted,
        interactObjectTargeted,
    }
    [HideInInspector]
    public PickupState currentPickupState = PickupState.noObjectTargeted;
    public float distance = 3;
    public float distanceMin = 2;
    public float distanceMax = 5;

    Vector3Int[] dropCoords = new Vector3Int[2];
    Vector3[] dropLocations = new Vector3[2];
    bool[] canDropAtCoords = new bool[] { true, true };

    [HideInInspector]
    public GameObject targetedObject = null;
    private Interaction targetedItemInteraction;
    private Box targetedItemBox;
    [HideInInspector]
    public GameObject carriedItem = null;
    public Stack<Box> carriedObjects = new Stack<Box>();
    public BoxInventoryDisplay boxInventoryDisplay;
    private bool wasPickupPressed;
    private bool wasDropOnStackPressed;
    private Rigidbody itemRigidbody;
    private Mesh carriedItemMesh;
    private Material carriedItemMaterial;
    [HideInInspector]
    public Grid placementGrid;
    //public MatchThreeGridDataStructure structure;
    public Fade prompt;
    public Fade exception;
    private int layerMaskPickup;
    private int layerMaskObstructed;
    private int layerMaskHide;

    void Awake()
    {
        layerMaskPickup = LayerMask.GetMask("InteractSolid", "InteractSoft", "Default");
        layerMaskObstructed = LayerMask.GetMask("Player");
        layerMaskHide = LayerMask.GetMask("InteractSolid", "Default");

        //character = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for collision with liftable objects
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
        bool objectFound = Physics.Raycast(ray, out hitInfo, distanceMax, layerMaskPickup);
        switch (currentPickupState)
        {
            case PickupState.noObjectTargeted:
                if (objectFound)
                {
                    targetedObject = hitInfo.transform.gameObject;
                    bool isBusy = true;
                    PickupState nextState = PickupState.noObjectTargeted;
                    switch (hitInfo.transform.tag)
                    {
                        case "Interactable":
                            nextState = PickupState.interactObjectTargeted;
                            targetedItemInteraction = targetedObject.GetComponent<Interaction>();
                            isBusy = targetedItemInteraction.isBusy;
                            RefreshText();
                            break;
                        case "Liftable":
                            nextState = PickupState.pickupObjectTargeted;
                            targetedItemBox = targetedObject.GetComponent<Box>();
                            isBusy = false;
                            prompt.fadeInText("LIFT");
                            
                            break;
                        default:
                            break;
                    }
                    //Check the availability of the targeted object
                    if (isBusy == false)
                    {
                        currentPickupState = nextState;
                        //Debug.Log("Target found.");
                        //Debug.Log("Targeted item: " + targetedObject.name);
                    }
                }


                break;
            case PickupState.interactObjectTargeted:
                if (InteractTargetLost(objectFound, hitInfo))
                {
                    //Object not targeted anymore
                    LoseTarget();
                }
                else
                {
                    if (Input.GetButtonDown("Interact/Pickup"))
                    {
                        //Interact with object
                        targetedItemInteraction.interact(this);
                    }
                }
                break;
            case PickupState.pickupObjectTargeted:
                if (PickupTargetLost(objectFound, hitInfo))
                {
                    LoseTarget();
                }
                else
                {
                    if (Input.GetButtonDown("Interact/Pickup"))
                    {
                        //Pickup object
                        PickupObject(targetedItemBox);
                    }
                    if (Input.GetButtonDown("PickupOnStack"))
                    {
                        //Pickup object on top of a stack
                        PickupObject(targetedItemBox.GetBoxOnTopOfMyStack());
                    }
                }
                break;
        }
        if(carriedObjects.Count > 0)
        { 
            //Check distance mod
            float scrollDelta = Input.GetAxis("DistanceModification");
            if (scrollDelta != 0)
            {
                distance = Mathf.Clamp(distance + scrollDelta, distanceMin, distanceMax);
            }

            //Set initial drop location
            dropCoords = new Vector3Int[] { Vector3Int.zero, Vector3Int.zero };
            dropLocations = new Vector3[] { Vector3.zero, Vector3.zero };
            canDropAtCoords = new bool[] { true, true };


            if (objectFound)
            {
                dropCoords[0] = placementGrid.WorldToCell(hitInfo.point + (hitInfo.normal * 0.5f));
                Box box = hitInfo.transform.gameObject.GetComponent<Box>();
                if (box != null)
                {
                    dropCoords[1] = box.GetBoxOnTopOfMyStack().coords[0] + Vector3Int.up;
                    canDropAtCoords[1] = Vector3.Distance(transform.position, placementGrid.CellToWorld(dropCoords[1])) < distanceMax;
                }
            }
            else
            {
                dropCoords[0] = placementGrid.WorldToCell(transform.position + (transform.rotation * (Vector3.forward * distance)));
                canDropAtCoords[1] = false;
            }

            //Convert back to world space
            for (int i = 0; i < dropCoords.Length; ++i)
            {
                dropLocations[i] = placementGrid.CellToWorld(dropCoords[i]);
                bool obstructionNotDetected = !Physics.CheckBox(dropLocations[i] + new Vector3(0.5f, 0.55f, 0.5f), new Vector3(0.51f, 0.475f, 0.51f), Quaternion.identity, layerMaskObstructed);
                bool clippingNotDetected = !Physics.CheckBox(dropLocations[i] + new Vector3(0.5f, 0.55f, 0.5f), new Vector3(0.51f, 0.475f, 0.51f), Quaternion.identity, layerMaskHide);
                canDropAtCoords[i] = canDropAtCoords[i] && obstructionNotDetected && clippingNotDetected;
                MaterialPropertyBlock properties = new MaterialPropertyBlock();
                if (clippingNotDetected)
                {
                    if (canDropAtCoords[i])
                    {
                        prompt.fadeInText("PLACE");
                        properties.SetColor("_Color", new Color(0.7f, 0.89f, 1f, 0.75f));
                    }
                    else
                    {
                        prompt.fadeOutText();
                        properties.SetColor("_Color", new Color(1f, 0.89f, 0.7f, 0.75f));
                    }
                    Graphics.DrawMesh(carriedItemMesh, dropLocations[i], Quaternion.identity, carriedItemMaterial, 0, GetComponent<Camera>(), 0, properties, false);
                }
            }
            if ((Input.GetButtonDown("Drop") && canDropAtCoords[0]) || (Input.GetButtonDown("DropOnStack") && !canDropAtCoords[1]))
            {
                DropObject(dropLocations[0]);
            }
            if (Input.GetButtonDown("DropOnStack") && canDropAtCoords[1])
            {
                DropObject(dropLocations[1]);
            }
        }
        
        


        //if Input.GetButtonDown("Pickup"){

        //}
    }
    private void FixedUpdate()
    {
        //switch (currentPickupState)
        //{


        //    case PickupState.carryingObject:
        //        if (wasPickupPressed)
        //        {
        //            DropObject(dropLocations[0]);
        //            wasPickupPressed = false;
        //        }
        //        if (wasDropOnStackPressed)
        //        {
        //            DropObject(dropLocations[1]);
        //            wasDropOnStackPressed = false;
        //        }
        //        break;
        //    case PickupState.constrainingObject:
        //        //Transfering object to another grid.
        //        Box m3_object = carriedItem.GetComponent<Box>();
        //        if (m3_object._Grid != placementGrid)
        //        {
        //            m3_object.RemoveMyself();
        //            m3_object._Grid = placementGrid;
        //            //m3_object.match3_grid = structure;
        //            m3_object.AddMyself(true);
        //            m3_object.transform.SetParent(placementGrid.transform);
        //        }
        //        itemRigidbody = carriedItem.GetComponent<Rigidbody>();
        //        m3_object.Frozen = false;
        //        currentPickupState = PickupState.noObjectTargeted;
        //        carriedItem = null;
        //        break;
        //}

    }

    public void PickupObject(Box lift)
    {
        //carriedItem = obj;
        carriedObjects.Push(lift);
        boxInventoryDisplay.AddBox(lift.groupId);
        SetCarriedItemMeshMaterialAndRigidbody(lift);
        itemRigidbody.velocity = Vector3.zero;
        // Debug.Log("Object carried: " + carried_item.name);
        lift.RemoveMyself();
        lift.gameObject.SetActive(false);
        //distance = distanceMax;
        //distance = Vector3.Distance(transform.position,obj.transform.position);

    }

    private void DropObject(Vector3 location)
    {
        Box droppedItem = carriedObjects.Pop();
        boxInventoryDisplay.RemoveBox();
        if(carriedObjects.Count > 0)
            SetCarriedItemMeshMaterialAndRigidbody(carriedObjects.Peek());
        //Transfering object to another grid.
        if (droppedItem._Grid != placementGrid)
        {
            droppedItem._Grid = placementGrid;
            droppedItem.transform.SetParent(placementGrid.transform);
        }
        droppedItem.AddMyself(true);
        droppedItem.gameObject.SetActive(true);
        //Remove constraints of rigidbody
        droppedItem.transform.position = location;
        droppedItem.transform.rotation = Quaternion.identity;
        
        droppedItem.Frozen = false;
    }

    private void SetCarriedItemMeshMaterialAndRigidbody(Box carriedItem)
    {
        carriedItemMesh = carriedItem.GetComponent<MeshFilter>().mesh;
        carriedItemMaterial = carriedItem.GetComponent<MeshRenderer>().sharedMaterials[0];
        itemRigidbody = carriedItem.GetComponent<Rigidbody>();
    }

    public void RefreshText()
    {
        prompt.fadeInText(targetedItemInteraction.getPrompt());
    }

    private bool InteractTargetLost(bool objectFound,RaycastHit hit)
    {
            return !objectFound || hit.transform.gameObject != targetedObject || targetedItemInteraction.isBusy == true;
    }

    private bool PickupTargetLost(bool objectFound,RaycastHit hit)
    {
            return !objectFound || hit.transform.gameObject != targetedObject;
    }

    private void LoseTarget()
    {
        //Object not targeted anymore
        prompt.fadeOutText();
        currentPickupState = PickupState.noObjectTargeted;
    }
}
