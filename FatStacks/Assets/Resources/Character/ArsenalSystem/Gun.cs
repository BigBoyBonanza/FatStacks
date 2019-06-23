using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GunData gunData;

    public int ammo;
    protected AudioSource gunAudioSource;

    private void Start()
    {
        gunAudioSource = GetComponentInParent<AudioSource>();
        ammo = Mathf.Clamp(ammo, 0, gunData.ammoCapacity);
    }
    public virtual void fire1(Ray ray)
    {

    }
    public virtual void fire2(Ray ray)
    {

    }

    public virtual float getAmmoFill()
    {
        return Mathf.Clamp(ammo / (float)gunData.ammoCapacity, 0f, 1f);
    }

    public virtual bool canFire()
    {
        return true;
    }

    public virtual int addAmmo(int amount)
    {
        ammo = ammo + amount;
        int leftover = Mathf.Max(ammo - gunData.ammoCapacity, 0);
        ammo -= leftover;
        return leftover;
    }

    protected void playFireSound(int index)
    {
        gunAudioSource.clip = gunData.shootSound[index];
        gunAudioSource.Play();
    }

    protected void playEmptySound(int index)
    {
        gunAudioSource.clip = gunData.emptySound[index];
        gunAudioSource.Play();
    }

    protected void playErrorSound(int index)
    {
        gunAudioSource.clip = gunData.errorSound[index];
        gunAudioSource.Play();
    }
}
