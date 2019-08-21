using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Gun
{
    public int rocketCount;
    public float spreadAngle;
    public GameObject Rocket;
    public float rocketFireVelocity = 1000;
    public Transform BarrelExit;
    List<Quaternion> rockets;
    public HealthManager ownerHealthManager;

    private void Awake()
    {
        rockets = new List<Quaternion>(rocketCount);
        for (int i = 0; i < rocketCount; i++)
        {
            //rockets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    private void Start()
    {
        ownerHealthManager = GetComponentInParent<HealthManager>();
    }

    public override void fire1(Ray ray)
    {
        //rockets[0] = Random.rotation;
        GameObject p = Instantiate(Rocket, BarrelExit.position, BarrelExit.rotation);
        //p.transform.rotation = Quaternion.RotateTowards(p.transform.rotation, rockets[0], spreadAngle);
        Rocket r = p.GetComponent<Rocket>();
        r.rocketSpeed = rocketFireVelocity;
        r.ownerHealthManager = ownerHealthManager;
        if(gunData.finiteAmmo)
            ammo--;
        //playFireSound(0);
    }

    public override bool canFire()
    {
        return ammo > 0;
    }
}
