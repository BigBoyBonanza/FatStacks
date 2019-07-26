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


    private void Awake()
    {
        rockets = new List<Quaternion>(rocketCount);
        for (int i = 0; i < rocketCount; i++)
        {
            rockets.Add(Quaternion.Euler(Vector3.zero));
        }
    }


    public override void fire1(Ray ray)
    {
        rockets[0] = Random.rotation;
        GameObject p = Instantiate(Rocket, BarrelExit.position, BarrelExit.rotation);
        p.transform.rotation = Quaternion.RotateTowards(p.transform.rotation, rockets[0], spreadAngle);
        p.GetComponent<Rigidbody>().AddForce(p.transform.right * rocketFireVelocity);

        ammo--;
        //playFireSound(0);
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
