using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCrateUnpacker : Interaction
{
    public Transform player;
    public float range;
    public Transform itemSpawnLocation;
    public Transform boxDropLocation;
    public GameObject rocketAmmo;
    public GameObject healthPack;
    public Animator animator;
    bool closeEnough;

    private void Awake()
    {
        closeEnough = Vector3.Distance(player.position, transform.position) < range;
        isBusy = !closeEnough;
    }

    private void Update()
    {
        if(player != null)
        {
            bool inRange = Vector3.Distance(player.position, transform.position) < range;
            if (inRange != closeEnough)
            {
                closeEnough = inRange;
                animator.SetBool("PlayerCloseEnough", closeEnough);
                isBusy = !closeEnough;
            }
        }
    }

    public override void Interact(Pickup pickup)
    {
        if(pickup.carriedObjects.Count > 0)
        {
            pickup.DropObject(boxDropLocation.position, boxDropLocation.rotation);
        }
        else
        {
            pickup.exception.FlashText(GetException(0), 3f);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Box box = other.GetComponent<Box>();
        if (box != null)
        {
            switch (box.i_am)
            {
                case "RocketAmmo":
                    SpawnRocketAmmo();
                    Destroy(box.gameObject);
                    break;
                case "Health":
                    SpawnHealth();
                    Destroy(box.gameObject);
                    break;
                default:
                    Destroy(box.gameObject);
                    break;
            }
        }
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<HealthManager>().Kill();
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
