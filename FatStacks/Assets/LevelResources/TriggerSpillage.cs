using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpillage : MonoBehaviour
{
    public BoxSpawner boxSpawnerSpill;
    public BoxSpawner boxSpawnerDrip;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            boxSpawnerSpill.TurnSpawnerOn(true);
            yield return new WaitUntil(()=>boxSpawnerSpill.amount == 0);
            boxSpawnerDrip.TurnSpawnerOn(true);
        }
    }
}
