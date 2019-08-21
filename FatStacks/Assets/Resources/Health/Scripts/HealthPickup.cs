using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Interaction
{
    public int amount;

    public override void interact(Pickup pickup)
    {
        //do nothing
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Pickup pickup = other.gameObject.GetComponentInChildren<Pickup>();
            HealthManager healthManager = other.gameObject.GetComponentInChildren<HealthManager>();
            if (healthManager.health == healthManager.maxHealth)
            {
                pickup.exception.FlashText(get_exception(0), 3);
            }
            else
            {
                healthManager.GiveHealth(amount);
                Destroy(gameObject);
            }
        }
    }
}
