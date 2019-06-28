using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Player Character;
    public ArsenalSystem gunController;
    public enum pickup_state
    {
        no_object_targeted,
        target_too_heavy,
        target_blocked,
        target_available,
        carrying_object,
        constraining_object
    }
    [HideInInspector]
    public pickup_state state = pickup_state.no_object_targeted;
    public float distance = 3;
    public float distanceMin = 2;
    public float distanceMax = 5;

    Vector3Int[] dropCoords = new Vector3Int[2];
    Vector3[] dropLocations = new Vector3[2];
    bool[] canDropAtCoords = new bool[] { true, true };

    [HideInInspector]
    public GameObject targeted_item = null;
    private Interaction targeted_item_interaction;
    [HideInInspector]
    public GameObject carried_item = null;
    private bool wasPickupPressed;
    private bool wasDropOnStackPressed;
    private Rigidbody item_rigidbody;
    [HideInInspector]
    public Grid placement_grid;
    //public MatchThreeGridDataStructure structure;
    public Fade prompt;
    public Fade exception;
    private Mesh carried_item_mesh;
    private Material carried_item_material;
    private int layerMaskPickup;
    private int layerMaskObstructed;
    private int layer_mask_hide;

    void Awake()
    {
        layerMaskPickup = LayerMask.GetMask("InteractSolid", "InteractSoft", "Default");
        layerMaskObstructed = LayerMask.GetMask("Player");
        layer_mask_hide = LayerMask.GetMask("InteractSolid", "Default");

        //character = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for collision with liftable objects
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
        bool objectFound = Physics.Raycast(ray, out hitInfo, distance, layerMaskPickup);
        switch (state)
        {
            case pickup_state.no_object_targeted:

                if (objectFound && hitInfo.transform.tag == "Interactable")
                {
                    //Check the availability of the targeted object
                    targeted_item = hitInfo.transform.gameObject;
                    targeted_item_interaction = targeted_item.GetComponent<Interaction>();
                    if (targeted_item_interaction.isBusy == false)
                    {
                        state = pickup_state.target_available;
                        RefreshText();
                    }
                    //Debug.Log("Target found.");
                    //Debug.Log("Targeted item: " + targeted_item.name);

                }
                break;
            case pickup_state.target_available:
                if (!objectFound || hitInfo.transform.gameObject != targeted_item || targeted_item_interaction.isBusy == true)
                {
                    //Object not targeted anymore
                    //Debug.Log("Target lost.");
                    prompt.fade_out_text();
                    state = pickup_state.no_object_targeted;
                }
                else
                {
                    if (Input.GetButtonDown("Pickup"))
                    {
                        //Carry object
                        targeted_item_interaction.interact(this);
                    }
                    if (Input.GetButtonDown("Drop On Stack"))
                    {
                        //Check if target object is a box
                        Box box = targeted_item_interaction.GetComponent<Box>();
                        if(box != null)
                        {
                            box.GetBoxOnTopOfMyStack().GetComponent<Interaction>().interact(this);
                        }
                    }
                }
                break;
            case pickup_state.carrying_object:
                //Check scroll
                float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
                if (scrollDelta != 0)
                {
                    distance = Mathf.Clamp(distance + Mathf.Sign(scrollDelta), distanceMin, distanceMax);
                }

                //Set initial drop location
                dropCoords = new Vector3Int[] { Vector3Int.zero, Vector3Int.zero };
                dropLocations = new Vector3[] { Vector3.zero, Vector3.zero };
                canDropAtCoords = new bool[] { true, true };

                
                if (objectFound)
                {
                    dropCoords[0] = placement_grid.WorldToCell(hitInfo.point + (hitInfo.normal * 0.5f));
                    Box box = hitInfo.transform.gameObject.GetComponent<Box>();
                    if (box != null)
                    {
                        dropCoords[1] = box.GetBoxOnTopOfMyStack().coord[0] + Vector3Int.up;
                        canDropAtCoords[1] = Vector3.Distance(transform.position, placement_grid.CellToWorld(dropCoords[1])) < distanceMax;
                    }
                }
                else
                {
                    dropCoords[0] = placement_grid.WorldToCell(transform.position + (transform.rotation * (Vector3.forward * distance)));
                    canDropAtCoords[1] = false;
                }
                
                //Convert back to world space
                for(int i = 0; i < dropCoords.Length; ++i)
                {
                    dropLocations[i] = placement_grid.CellToWorld(dropCoords[i]);
                    bool obstructionNotDetected = !Physics.CheckBox(dropLocations[i] + new Vector3(0.5f, 0.55f, 0.5f), new Vector3(0.51f, 0.475f, 0.51f), Quaternion.identity, layerMaskObstructed);
                    bool clippingNotDetected = !Physics.CheckBox(dropLocations[i] + new Vector3(0.5f, 0.55f, 0.5f), new Vector3(0.51f, 0.475f, 0.51f), Quaternion.identity, layer_mask_hide);
                    canDropAtCoords[i] = canDropAtCoords[i] && obstructionNotDetected && clippingNotDetected;
                    MaterialPropertyBlock properties = new MaterialPropertyBlock();
                    if (clippingNotDetected)
                    {
                        if (canDropAtCoords[i])
                        {
                            prompt.FadeInText("PLACE");
                            properties.SetColor("_Color", new Color(0.7f, 0.89f, 1f, 0.75f));
                        }
                        else
                        {
                            prompt.fade_out_text();
                            properties.SetColor("_Color", new Color(1f, 0.89f, 0.7f, 0.75f));
                        }
                        Graphics.DrawMesh(carried_item_mesh, dropLocations[i], Quaternion.identity, carried_item_material, 0, GetComponent<Camera>(), 0, properties, false);
                    }
                }
                if (Input.GetButtonDown("Pickup") && canDropAtCoords[0])
                {

                    wasPickupPressed = true;
                }
                if (Input.GetButtonDown("Drop On Stack") && canDropAtCoords[1])
                {

                    wasDropOnStackPressed = true;
                }
                break;
        }



        //if Input.GetButtonDown("Pickup"){

        //}
    }
    private void FixedUpdate()
    {
        switch (state)
        {


            case pickup_state.carrying_object:
                if (wasPickupPressed)
                {
                    DropObject(dropLocations[0]);
                    wasPickupPressed = false;
                }
                if (wasDropOnStackPressed)
                {
                    DropObject(dropLocations[1]);
                    wasDropOnStackPressed = false;
                }
                break;
            case pickup_state.constraining_object:
                //Transfering object to another grid.
                Box m3_object = carried_item.GetComponent<Box>();
                if (m3_object._Grid != placement_grid)
                {
                    m3_object.RemoveMyself();
                    m3_object._Grid = placement_grid;
                    //m3_object.match3_grid = structure;
                    m3_object.AddMyself(true);
                    m3_object.transform.SetParent(placement_grid.transform);
                }
                item_rigidbody = carried_item.GetComponent<Rigidbody>();
                m3_object.Frozen = false;
                state = pickup_state.no_object_targeted;
                carried_item = null;
                break;
        }

    }

    public void PickupObject(GameObject obj)
    {
        state = pickup_state.carrying_object;
        carried_item = obj;
        carried_item_mesh = carried_item.GetComponent<MeshFilter>().mesh;
        carried_item_material = carried_item.GetComponent<MeshRenderer>().sharedMaterials[0];
        item_rigidbody = carried_item.GetComponent<Rigidbody>();
        item_rigidbody.velocity = Vector3.zero;
        // Debug.Log("Object carried: " + carried_item.name);
        carried_item.SetActive(false);
        distance = distanceMax;
        //distance = Vector3.Distance(transform.position,obj.transform.position);

    }
    private void DropObject(Vector3 location)
    {
        carried_item.SetActive(true);
        //Remove constraints of rigidbody
        item_rigidbody.constraints = RigidbodyConstraints.None;
        item_rigidbody.MovePosition(location);
        //Reapply constraints on the next frame
        state = pickup_state.constraining_object;
        distance = distanceMax;
    }
    public void RefreshText()
    {
        prompt.FadeInText(targeted_item_interaction.get_prompt());
    }
}
