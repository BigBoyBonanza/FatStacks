using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHealthManager : HealthManager
{
    public override void Kill()
    {
        base.Kill();
        Destroy(gameObject);
    }
}
