using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData : SaveData
{
    public int health;
    public bool[] inArse;
    public int[] ammo;
    public int equip;
    public SerializableVector3 position;
    public SerializableQuaternion orientation;
    public bool crouched;

    public override void Write(Save save, SaveObject saveObject)
    {

        //Get player
        Player player = saveObject.GetComponent<Player>();

        //Get health
        health = player.Health;

        //Get arsenal info
        ArsenalSystem gunController = player.GetComponentInChildren<ArsenalSystem>();
        ammo = new int[gunController.arsenal.Length];
        inArse = new bool[gunController.arsenal.Length];
        equip = gunController.equipped_gun_index;
        for (int i = 0; i < ammo.Length; ++i)
        {
            ammo[i] = gunController.arsenal[i]._gun.ammo;
            inArse[i] = gunController.arsenal[i].isInArsenal;
        }

        //Get position, orientation, etc.
        crouched = player.GetCrouched();
        orientation = player._Camera.transform.rotation;
        position = player.transform.position;
    }

    public override void Read()
    {
        //Instantiate a player
        Player player = UnityEngine.Object.Instantiate(SaveSystem.playerObject).GetComponent<Player>();

        //Set save system player to newly instantiated player
        SaveSystem.player = player;

        //Set position, orientation, etc.
        player.transform.position = position;
        player._Camera.GetComponent<MouseLook>().Look(((Quaternion)orientation).eulerAngles);
        player.SetCrouchState(crouched, true);

        //Set health
        player.Health = health;

        //Set arsenal
        ArsenalSystem gunController = player.myPickup.gunController;
        for (int i = 0; i < ammo.Length; ++i)
        {
            gunController.arsenal[i]._gun.ammo = ammo[i];
            gunController.arsenal[i].isInArsenal = inArse[i];
        }
        gunController.equip_gun((ArsenalSystem.GunType)equip);
    }
}

