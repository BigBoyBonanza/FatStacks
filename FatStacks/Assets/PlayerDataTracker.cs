using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataTracker : MonoBehaviour
{
    public static PlayerDataTracker i;
    
    public int health;
    public bool[] inArse;
    public int[] ammo;
    public int equip;
    //public Stack<string> inventory;

    void Awake()
    {
        if (!i)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    
    public void WriteFrom(Player player)
    {
        //Get health
        health = player.healthManager.health;

        //Get arsenal info
        ArsenalSystem gunController = player.GetComponentInChildren<ArsenalSystem>();
        ammo = new int[gunController.arsenal.Length];
        inArse = new bool[gunController.arsenal.Length];
        equip = gunController.equippedGunIndex;
        for (int i = 0; i < ammo.Length; ++i)
        {
            ammo[i] = gunController.arsenal[i]._gun.ammo;
            inArse[i] = gunController.arsenal[i].isInArsenal;
        }

        ////Get inventory
        //foreach(Box box in player.myPickup.carriedObjects)
        //{
        //    inventory.Push(box.resourcePath);
        //}
    }

    public void ReadTo(ref Player player)
    {
        //Set health
        player.healthManager.health = health;

        //Set arsenal
        ArsenalSystem gunController = player.myPickup.gunController;
        for (int i = 0; i < ammo.Length; ++i)
        {
            gunController.arsenal[i]._gun.ammo = ammo[i];
            gunController.arsenal[i].isInArsenal = inArse[i];
        }
        gunController.EquipGun((ArsenalSystem.GunType)equip);
    }
}
