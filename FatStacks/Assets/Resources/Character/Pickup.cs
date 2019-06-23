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
    public float distance = 1;
    [HideInInspector]
    public GameObject targeted_item = null;
    private Interaction targeted_item_interaction;
    [HideInInspector]
    public GameObject carried_item = null;
    private bool was_pickup_pressed;
    private Vector3 finalDropLocation;
    private Rigidbody item_rigidbody;
    [HideInInspector]
    public Grid placement_grid;
    //public MatchThreeGridDataStructure structure;
    public Fade prompt;
    public Fade exception;
    private Mesh carried_item_mesh;
    private Material carried_item_material;
    private int layer_mask_pickup;
    private int layer_mask_obstructed;
    private int layer_mask_hide;

    void Awake()
    {
        layer_mask_pickup = LayerMask.GetMask("InteractSolid", "InteractSoft", "Default");
        layer_mask_obstructed = LayerMask.GetMask("Player");
        layer_mask_hide = LayerMask.GetMask("InteractSolid", "Default");

        //character = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for collision with liftable objects
        RaycastHit hit_info;
        Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
        //Debug.DrawRay(ray.origin, ray.direction, Color.green);
        bool object_found = Physics.Raycast(ray, out hit_info, distance, layer_mask_pickup);
        switch (state)
        {
            case pickup_state.no_object_targeted:

                if (object_found && hit_info.transform.tag == "Interactable")
                {
                    //Check the availability of the targeted object
                    targeted_item = hit_info.transform.gameObject;
                    targeted_item_interaction = targeted_item.GetComponent<Interaction>();
                    if (targeted_item_interaction.isBusy == false)
                    {
                        state = pickup_state.target_available;
                        refresh_text();
                    }
                    //Debug.Log("Target found.");
                    //Debug.Log("Targeted item: " + targeted_item.name);

                }
                break;
            case pickup_state.target_available:
                if (!object_found || hit_info.transform.gameObject != targeted_item || targeted_item_interaction.isBusy == true)
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
                        /*
                        state = pickup_state.carrying_object;
                        carried_item = targeted_item;
                        carried_item_mesh = carried_item.GetComponent<MeshFilter>().mesh;
                        carried_item_material = carried_item.GetComponent<MeshRenderer>().material;
                        item_rigidbody = carried_item.GetComponent<Rigidbody>();
                        Debug.Log("Object carried: " + carried_item.name);
                        
                        carried_item.SetActive(false);
                        */
                    }
                }
                break;
            case pickup_state.carrying_object:
                //Set initial drop location
                Vector3 initDropLocation = new Vector3();
                if (object_found)
                {
                    initDropLocation = hit_info.point + (hit_info.normal * 0.5f);
                }
                else
                {
                    initDropLocation = transform.position + (transform.rotation * (Vector3.forward * distance));
                }
                Vector3Int dropCoords = placement_grid.WorldToCell(initDropLocation);
                //Convert back to world space
                finalDropLocation = placement_grid.CellToWorld(dropCoords);
                bool can_drop = !Physics.CheckBox(finalDropLocation + new Vector3(0.5f, 0.55f, 0.5f), new Vector3(0.51f, 0.475f, 0.51f), Quaternion.identity, layer_mask_obstructed);
                bool show = !Physics.CheckBox(finalDropLocation + new Vector3(0.5f, 0.55f, 0.5f), new Vector3(0.51f, 0.475f, 0.51f), Quaternion.identity, layer_mask_hide);
                can_drop = can_drop && show;
                MaterialPropertyBlock properties = new MaterialPropertyBlock();
                if (show)
                {
                    if (can_drop)
                    {
                        prompt.FadeInText("PLACE");
                        properties.SetColor("_Color", new Color(0.7f, 0.89f, 1f, 0.75f));
                    }
                    else
                    {
                        prompt.fade_out_text();
                        properties.SetColor("_Color", new Color(1f, 0.89f, 0.7f, 0.75f));
                    }
                    Graphics.DrawMesh(carried_item_mesh, finalDropLocation, Quaternion.identity, carried_item_material, 0, GetComponent<Camera>(), 0, properties, false);
                }
                if (Input.GetButtonDown("Pickup") && can_drop)
                {
                    //Set carried item down
                    // Debug.Log(drop_coords);
                    //Debug.Log("Object dropped: " + carried_item.name);
                    //Debug.Log("Location: " + drop_location);
                    //
                    was_pickup_pressed = true;

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
                if (was_pickup_pressed)
                {
                    drop_object();
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
                item_rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                state = pickup_state.no_object_targeted;
                carried_item = null;
                break;
        }

    }

    public void pickup_object(GameObject obj)
    {
        state = pickup_state.carrying_object;
        carried_item = obj;
        carried_item_mesh = carried_item.GetComponent<MeshFilter>().mesh;
        carried_item_material = carried_item.GetComponent<MeshRenderer>().sharedMaterials[1];
        item_rigidbody = carried_item.GetComponent<Rigidbody>();
        // Debug.Log("Object carried: " + carried_item.name);
        carried_item.SetActive(false);


    }
    private void drop_object()
    {
        carried_item.SetActive(true);
        //Remove constraints of rigidbody
        item_rigidbody.constraints = RigidbodyConstraints.None;
        item_rigidbody.MovePosition(finalDropLocation);
        //Reapply constraints on the next frame
        state = pickup_state.constraining_object;
        was_pickup_pressed = false;
    }
    public void refresh_text()
    {
        prompt.FadeInText(targeted_item_interaction.get_prompt());
    }
}
