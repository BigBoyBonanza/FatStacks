using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HealthManager manager = other.gameObject.GetComponent<HealthManager>();
        if(manager != null)
        {
            manager.Kill();
        }
    }
}
