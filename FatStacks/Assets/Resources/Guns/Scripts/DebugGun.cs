using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGun : Gun
{
    
    public override void fire1(Ray ray)
    {
        RaycastHit hit_info;
        bool object_found = Physics.Raycast(ray, out hit_info, float.MaxValue, LayerMask.GetMask("Default", "InteractSolid"));
        if (object_found && hit_info.transform.tag == "Liftable")
        {
            Box box = hit_info.transform.gameObject.GetComponent<Box>();
            for (int j = 0; j < box.match3_coord_evaluator_local_transforms.Length; ++j)
            {
                Debug.DrawRay(box.transform.position + box.transform.rotation * box.match3_coord_evaluator_local_transforms[j], Vector3.up, Color.red, 5f);
            }
            string message = "Coords: \n";
            for (int i = 0; i < box.coords.Length; ++i)
            {
                message += box.coords[i];
                message += "\n";
            }
            Debug.Log(message);
        }
    }
    
}
