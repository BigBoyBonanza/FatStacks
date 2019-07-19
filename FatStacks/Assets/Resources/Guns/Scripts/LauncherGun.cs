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
        Box box = hit_info.transform?.GetComponent<Box>();
        if (object_found && box != null)
        {
            while(true)
            {
                Rigidbody rigidbody = box.GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
                Box nextBox = box.GetBoxOnTopOfMe();
                if(box != nextBox && nextBox != null)
                {
                    box = nextBox;
                }
                else
                {
                    break;
                }
            }
            ammo -= 1;
        }
        
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
