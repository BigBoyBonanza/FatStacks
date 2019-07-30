using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    public float force = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            ConstantForce constantForce = collision.gameObject.AddComponent<ConstantForce>();
            constantForce.force = force * -transform.right;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        ConstantForce constantForce = collision.gameObject.GetComponent<ConstantForce>();
        if (collision.gameObject.GetComponent<ConstantForce>() != null)
        {
            Destroy(constantForce);
        }
    }
}
