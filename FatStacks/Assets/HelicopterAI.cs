using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterAI : MonoBehaviour
{
    public float initVelocity;
    public Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody.velocity = transform.rotation * Vector3.forward * initVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(transform.position, collision.GetContact(0).normal,Color.white,5f);
        rigidbody.velocity = Vector3.Reflect(rigidbody.velocity.normalized, collision.GetContact(0).normal) * rigidbody.velocity.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.MovePosition(transform.position + rigidbody.velocity * Time.deltaTime);
    }
}
