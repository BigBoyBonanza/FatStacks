using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBossHealthManager : HealthManager
{
    
    int stage = 0;
    public int[] healthStage = new int[3];
    [SerializeField]
    public HelicopterBossStage[] stages = new HelicopterBossStage[3];
    public HelicopterBossAI Helicopter;
    public BoxSpawner boxSpawner;
    public BoxSpawner rocketBoxSpawner;
    [System.Serializable]
    public struct HelicopterBossStage
    {
        public float helicopterSpeed;
        public float helicopterFireRate;
        public float boxSpawnRate;
        public float rocketBoxSpawnRate;
    };

    private void Start()
    {
        SetUpStage(0);
    }

    public override int DealDamage(int amount)
    {
        base.DealDamage(amount);
        if (health < healthStage[stage])
        {
            ++stage;
            SetUpStage(stage);
        }
        return 0;
    }

    public void SetUpStage(int index)
    {
        HelicopterBossStage stage = stages[index];
        Helicopter.fireRate = stage.helicopterFireRate;
        Helicopter.speed = stage.helicopterSpeed;
        boxSpawner.minInterval = stage.boxSpawnRate;
        rocketBoxSpawner.minInterval = stage.rocketBoxSpawnRate;
    }

    public override void Kill()
    {
        Destroy(gameObject);
    }
}
