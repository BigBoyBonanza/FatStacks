using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherGun : Gun
{
    public override void fire1(Ray ray)
    {
        //RoomResetterInteraction.canReset = true;
        RaycastHit hit_info;
        bool object_found = Physics.Raycast(ray, out hit_info, float.MaxValue, LayerMask.GetMask("Default", "InteractSolid"));
        Box box = hit_info.transform.GetComponent<Box>();
        if (object_found && box != null)
        {
            while(box != null)
            {
                Rigidbody rigidbody = box.GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
                box = box.GetBoxOnTopOfMe();
            }
            
        }
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
