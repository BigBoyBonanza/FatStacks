using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Gun
{
    public int pelletCount;
    public float spreadAngle;
    public GameObject pellet;
    public float pelletFireVelocity = 1000;
    public Transform BarrelExit;
    List<Quaternion> pellets;


    private void Awake()
    {
        pellets = new List<Quaternion>(pelletCount);
        for (int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }
    }


    public override void fire1(Ray ray)
    {
        for (int i = 0; i < pelletCount; i++)
        {
            pellets[i] = Random.rotation;
            GameObject p = Instantiate(pellet, BarrelExit.position, BarrelExit.rotation);
            p.transform.rotation = Quaternion.RotateTowards(p.transform.rotation, pellets[i], spreadAngle);
            p.GetComponent<Rigidbody>().AddForce(p.transform.right * pelletFireVelocity);
        }

        ammo--;
        //playFireSound(0);
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
