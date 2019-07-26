using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{
    public float blastRadius = 5.0f;
    public float blastPower = 10.0f;

    private void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, blastRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(blastPower, explosionPos, blastRadius, 3.0F);
        }
    }

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
