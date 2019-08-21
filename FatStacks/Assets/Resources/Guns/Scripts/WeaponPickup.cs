using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interaction
{
    public ArsenalSystem.GunType gun;
    public int ammo;

    public override void interact(Pickup pickup)
    {
        //do nothing
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ArsenalSystem arsenal = other.gameObject.GetComponentInChildren<ArsenalSystem>();
            arsenal.AddGunToArsenalAndEquip(gun);
            arsenal.AddAmmoToGun(gun, ammo);
            Destroy(gameObject);
        }
    }
}
