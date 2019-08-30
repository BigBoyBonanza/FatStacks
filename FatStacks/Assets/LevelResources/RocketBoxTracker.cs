using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBoxTracker : MonoBehaviour
{
    public RocketBoxSpawner spawner;

    private void OnDestroy()
    {
        spawner.liveRockets -= 1;
    }
}
