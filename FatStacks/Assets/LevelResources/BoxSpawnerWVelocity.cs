using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxSpawnerWVelocity : BoxSpawner
{
    public Vector2 xForce;
    public Vector2 yForce;
    public Vector2 zForce;

    protected override IEnumerator SpawnBox()
    {
        System.Random random = new System.Random();
        while (on && amount > 0)
        {
            GameObject obj = Instantiate(pool[random.Next(pool.Length)].gameObject, transform.position, Quaternion.identity, boxGrid);
            obj.SetActive(true);
            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            rigidbody.AddForce(new Vector3(random.Next((int)xForce.x, (int)xForce.y), random.Next((int)yForce.x, (int)yForce.y), random.Next((int)zForce.x, (int)zForce.y)),ForceMode.VelocityChange);
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
