using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBoxSpawner : BoxSpawnerWVelocity
{
    public int firstRocketInPool;
    public int maxRocketCrates;
    [HideInInspector]
    public int liveRockets;

    protected override IEnumerator SpawnBox()
    {
        System.Random random = new System.Random();
        while (on && amount > 0)
        {
            int selection = random.Next(pool.Length);
            if (selection >= firstRocketInPool && liveRockets < maxRocketCrates)
            {
                GameObject obj = Instantiate(pool[selection].gameObject, transform.position, Quaternion.identity, boxGrid);

                //Increment Live Rockets
                RocketBoxTracker tracker = obj.AddComponent<RocketBoxTracker>();
                tracker.spawner = this;
                liveRockets += 1;
               
                obj.SetActive(true);
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                rigidbody.AddForce(new Vector3(random.Next((int)xForce.x, (int)xForce.y), random.Next((int)yForce.x, (int)yForce.y), random.Next((int)zForce.x, (int)zForce.y)), ForceMode.VelocityChange);
            }
            else if(selection < firstRocketInPool)
            {
                GameObject obj = Instantiate(pool[selection].gameObject, transform.position, Quaternion.identity, boxGrid);
                obj.SetActive(true);
                Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
                rigidbody.AddForce(new Vector3(random.Next((int)xForce.x, (int)xForce.y), random.Next((int)yForce.x, (int)yForce.y), random.Next((int)zForce.x, (int)zForce.y)), ForceMode.VelocityChange);
            }

            if (isFinite)
                amount -= 1;
            if (fixedRate)
                yield return new WaitForSeconds(minInterval);
            else
                yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
        StopCoroutine(coroutine);
    }
}
