using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public int pelletCount;
    public float spreadAngle;
    public GameObject pellet;
    public float pelletFireVelocity = 1;
    //setup barrelexit unless it just works
    public Transform barrelExit;
    List<Quaternion> pellets;

    //var projectile : Rigidbody;
    //public int speed = 20;

    
    private void Awake()
    {
        pellets = new List<Quaternion>(pelletCount);
        for(int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }
    }
    

    public override void fire1(Ray ray)
    {

        //TODO get shotgun to aim projectiles where crosshair's aiming
        for (int i = 0; i < pelletCount; i++)
        {
            pellets[i] = Random.rotation;
            GameObject p = Instantiate(pellet, barrelExit.position, barrelExit.rotation);
            p.transform.rotation = Quaternion.RotateTowards(p.transform.rotation, pellets[i], spreadAngle);
            p.GetComponent<Rigidbody>().AddForce(p.transform.right * pelletFireVelocity);
        }
        
        ammo--;
        playFireSound(0);
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
