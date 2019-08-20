using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Interaction
{
    public ArsenalSystem.GunType gun;
    public int amount;

    public override void interact(Pickup pickup)
    {
        //do nothing
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Pickup pickup = other.gameObject.GetComponentInChildren<Pickup>();
            int oldAmount = amount;
            amount = pickup.gunController.AddAmmoToGun(gun, amount);
            if (amount == 0)
            {
                Destroy(gameObject);
            }
            else if (oldAmount == amount)
            {
                pickup.exception.FlashText(get_exception(0), 3);
            }
        }
    }
}
