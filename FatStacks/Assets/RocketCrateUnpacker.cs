using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCrateUnpacker : MonoBehaviour
{
    public Transform rocketSpawnLocation;
    public string rocketAmmoResourcePath;

    private void OnTriggerEnter(Collider other)
    {
        Box box = other.gameObject.GetComponent<Box>();
        if(box != null && box.i_am == "RocketAmmo")
        {
            SpawnRocketAmmo();
            Destroy(box.gameObject);
        }
    }

    private void SpawnRocketAmmo()
    {
        GameObject ammo = (GameObject)Resources.Load(rocketAmmoResourcePath);
        ammo.transform.position = rocketSpawnLocation.position;
        Instantiate(ammo);
    }
}
