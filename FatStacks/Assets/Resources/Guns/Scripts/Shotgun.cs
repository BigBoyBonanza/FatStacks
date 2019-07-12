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
    List<Quaternion> pellets;

    /*
    private void Awake()
    {
        pellets = new List<Quaternion>(pelletCount);
        for(int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }
    }
    */

    public override void fire1(Ray ray)
    {
        /*
        for (int i = 0; i < pelletCount; i++)
        {
            pellets[i] = Random.rotation;
            GameObject p = Instantiate(pellet, BARRELEXIT.Position, BARRELEXIT.rotation);
            p.transform.rotation = Quaternion.RotateTowards(p.rotation, pellets[i], spreadAngle);
            p.GetComponent<RigidBody>().AddForce(p.transform.right * pelletFireVelocity)
        }
        */
    }
    public override bool canFire()
    {
        return ammo > 0;
    }
}
