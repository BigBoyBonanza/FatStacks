using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public float initSpeed;
    public float Speed { get { return GetComponent<Animator>().speed; } set { GetComponent<Animator>().speed = value; } }

    private void Start()
    {
        Speed = initSpeed;
    }
};
