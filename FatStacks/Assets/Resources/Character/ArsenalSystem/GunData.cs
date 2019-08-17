using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun Info", fileName = "New gun info")]
public class GunData : ScriptableObject {

    public new string name;
    public bool finiteAmmo = true;
    public int ammoCapacity;
    public Sprite gun_sprite;
    public AudioClip[] shootSound;
    public AudioClip[] emptySound;
    public AudioClip[] errorSound;
}
