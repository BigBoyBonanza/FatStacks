using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{
    Rigidbody rigidbody;
    public float rocketSpeed;
    public float blastRadius = 5.0f;
    public float blastPower = 10.0f;

    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public new void FixedUpdate()
    {
        base.FixedUpdate();
        rigidbody.MovePosition(transform.position + (transform.rotation * Vector3.back/*Rocket model is backward*/ * rocketSpeed * Time.deltaTime));
    }

    public override void Hit(GameObject obj)
    {
        base.Hit(obj);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, blastRadius);
        HashSet<HealthManager> healthManagers = new HashSet<HealthManager>();
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            HealthManager healthManager = hit.GetComponentInParent<HealthManager>();
            DestructibleWall destructibleWall = hit.GetComponent<DestructibleWall>();
            int amount = (int)(damage * Vector3.Distance(transform.position, hit.transform.position) / blastRadius);
            if (rb != null)
                rb.AddExplosionForce(blastPower, explosionPos, blastRadius, 1F, ForceMode.VelocityChange);
            if (healthManager != null && (healthManager != ownerHealthManager || healthManager.selfDamage) && !healthManagers.Contains(healthManager))
            {
                healthManager.DealDamage(damage);
                healthManagers.Add(healthManager);
            }
            if (destructibleWall != null)
            {
                Destroy(destructibleWall.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
