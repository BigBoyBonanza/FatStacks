using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem particleSystem;

    //private void Start()
    //{
    //    Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    //}

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => particleSystem.IsAlive() == false);
        Destroy(gameObject);
    }
}
