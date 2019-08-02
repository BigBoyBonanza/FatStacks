using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBeltMalfunction : MonoBehaviour
{
    public AudioSource crash;
    public AudioSource malfunctioning;
    public BoxSpawner[] spawners;
    public Gear[] gears;

    public bool notTriggered = true;

    private void OnTriggerEnter(Collider collision)
    {
        if (notTriggered)
        {
            crash.Play();
            malfunctioning.Play();
            foreach (BoxSpawner spawner in spawners)
            {
                spawner.TurnSpawnerOn(true);
            }
            foreach (Gear gear in gears)
            {
                gear.Speed *= 5;
            }
            notTriggered = false;
        }
        
        //Destroy(gameObject);
    }
}
