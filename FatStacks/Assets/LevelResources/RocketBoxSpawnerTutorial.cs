using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBoxSpawnerTutorial : RocketBoxSpawner
{
    public ArsenalSystem arsenal;
    int tally = 0;

    private void Start()
    {
        StartCoroutine("TallyRockets");
    }

    private void Update()
    {
        if(tally < 1 && on == false)
        {
            TurnSpawnerOn();
        }
        else if(tally >= 1 && on == true)
        {
            TurnSpawnerOn(false);
        }
    }

    IEnumerator TallyRockets()
    {
        while (true)
        {
            tally = 0;
            tally += arsenal.arsenal[(int)ArsenalSystem.GunType.bazooka]._gun.ammo;
            tally += FindObjectsOfType<AmmoPickup>().Length;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
