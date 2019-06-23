using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGun : Gun
{
    
    public override void fire1(Ray ray)
    {
        RaycastHit hit_info;
        bool object_found = Physics.Raycast(ray, out hit_info, float.MaxValue, LayerMask.GetMask("Default", "InteractSolid"));
        if (object_found && hit_info.transform.tag == "Interactable")
        {
            Box box = hit_info.transform.gameObject.GetComponent<Box>();
            string message = "Coords: \n";
            for (int i = 0; i < box.coord.Length; ++i)
            {
                message += box.coord[i];
                message += "\n";
            }
            Debug.Log(message);
        }
    }
    
}
