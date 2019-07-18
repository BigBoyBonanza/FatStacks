using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPellet : Projectile
{
    public override void Hit(GameObject obj)
    {
        base.Hit(obj);
        HealthManager healthManager = obj.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            healthManager.DealDamage(damage);
        }
    }
}
