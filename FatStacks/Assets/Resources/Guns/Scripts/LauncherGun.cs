using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherGun : Gun
{
    public override void fire1(Ray ray)
    {
        //RoomResetterInteraction.canReset = true;
        RaycastHit hit_info;
        bool object_found = Physics.Raycast(ray, out hit_info, float.MaxValue, LayerMask.GetMask("Default", "InteractSolid", "Helicopter"));
        Box box = hit_info.transform?.GetComponent<Box>();
        if (object_found && box != null)
        {
            int tally = 1;
            while(true)
            {
                Rigidbody rigidbody = box.GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
                Box nextBox = box.GetBoxOnTopOfMe();
                if(box != nextBox && nextBox != null)
                {
                    tally += 1;
                    box = nextBox;
                }
                else
                {
                    break;
                }
            }
            if(tally == 1)
            {
                playFireSound(0);
            }
            else
            {
                playFireSound(1);
            }
            //ammo -= 1;
        }
        
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
