using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCrateUnpacker : MonoBehaviour
{
    public Transform itemSpawnLocation;
    public GameObject rocketAmmo;
    public GameObject healthPack;

    private void OnTriggerEnter(Collider other)
    {
        Box box = other.gameObject.GetComponent<Box>();
        if(box != null)
        {
            switch (box.i_am)
            {
                case "RocketAmmo":
                    SpawnRocketAmmo();
                    Destroy(box.gameObject);
                    break;
                case "Health":
                    SpawnHealth();
                    break;
                default:
                    Destroy(gameObject);
                    break;
            }
        }
        
    }
    private void SpawnRocketAmmo()
    {
        GameObject ammo = Instantiate(rocketAmmo);
        ammo.transform.position = itemSpawnLocation.position;
        
    }

    private void SpawnHealth()
    {
        GameObject health = Instantiate(healthPack);
        health.transform.position = itemSpawnLocation.position;
    }
}
