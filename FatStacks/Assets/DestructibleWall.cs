using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rocket rocket = collision.gameObject.GetComponent<Rocket>();
        if (rocket != null)
        {
            Destroy(gameObject);
        }
    }
}
