using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGun : Gun
{

    public override void fire1(Ray ray){

        //RoomResetterInteraction.canReset = true;
        RaycastHit hit_info;
        bool object_found = Physics.Raycast(ray, out hit_info, float.MaxValue, LayerMask.GetMask("Default", "InteractSolid"));
        if (object_found && hit_info.transform.GetComponent<Box>() != null)
        {
            // Debug.Log("Object shot: " + hit_info.transform.name);
            List<Box> boxs = hit_info.transform.gameObject.GetComponent<Box>().GetMatchingNeighbors();
            int count = boxs.Count;
            if (count >= 3)
            {
                if (count < 6)
                {
                    playFireSound(0);
                    ammo -= 1;
                }
                else
                {
                    playFireSound(1);
                }
                //Debug.Log("Group greater than or equal to three");
                foreach (Box item in boxs)
                {
                    //Instantiate(item._BoxMaterials.destructionPrefab[(int)item.match3_group_id], item.transform.position,Quaternion.identity);
                    Destroy(item.gameObject);
                }
                
            }
            else
            {
                playErrorSound(0);
                //Debug.Log("Group less than three");
            }
        }
        else
        {
            playErrorSound(0);
        }
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
