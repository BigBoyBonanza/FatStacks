using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterHealthManager : HealthManager
{
    public override void Kill()
    {
        base.Kill();
        Destroy(gameObject);
    }
}
