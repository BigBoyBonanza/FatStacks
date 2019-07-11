using UnityEngine;

public class Shotgun : Gun
{
    public override void fire1(Ray ray)
    {
        //RoomResetterInteraction.canReset = true;
        RaycastHit hit_info;
        bool object_found = Physics.Raycast(ray, out hit_info, float.MaxValue, LayerMask.GetMask("Default", "InteractSolid"));
        Box box = hit_info.transform?.GetComponent<Box>();
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
