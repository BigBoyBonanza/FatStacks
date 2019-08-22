using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFightTrigger : MonoBehaviour
{
    public HelicopterBossAI helicopter;
    public BoxSpawner boxSpawner;
    public BoxSpawner rocketBoxSpawner;
    public GameObject Wall;

    public bool notTriggered = true;

    private void OnTriggerEnter(Collider collision)
    {
        if (notTriggered)
        {
            helicopter.currState = HelicopterBossAI.State.flyingForward;
            boxSpawner.TurnSpawnerOn();
            rocketBoxSpawner.TurnSpawnerOn();
            Wall.SetActive(true);
            notTriggered = false;
        }

        //Destroy(gameObject);
    }
}
