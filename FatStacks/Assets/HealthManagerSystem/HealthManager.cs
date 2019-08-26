using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public bool clampHealthToMax = true;
    public bool selfDamage;
    public virtual int GiveHealth(int amount)
    {
        health += amount;
        if(health > maxHealth)
        {
            if (clampHealthToMax)
                ClampHealthToMax();
            return health - maxHealth;
        }
        return 0;   
    }
    
    public virtual int DealDamage(int amount, HealthManager attacker = null)
    {
        if (this != attacker || selfDamage)
        {
            health -= amount;
            if (health < 0)
            {
                Kill();
                return -health;
            }
        }
        return 0;
    }

    public void ClampHealthToMax()
    {
        health = maxHealth;
    }

    public virtual void Kill()
    {
        //Debug.Log(gameObject + " killed.");
        return;
    }
}
