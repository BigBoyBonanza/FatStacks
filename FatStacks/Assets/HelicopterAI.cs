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
        Vector3 direction = Vector3.zero;
        ContactPoint[] contactPoints = new ContactPoint[1];
        collision.GetContacts(contactPoints);
        foreach (ContactPoint contactPoint in contactPoints)
        {
            Vector3 collisionPosition = collision.GetContact(0).point;
            //Y
            if (collisionPosition.y > transform.position.y)
            {
                direction += Vector3.down;
            }
            else if (collisionPosition.y < transform.position.y)
            {
                direction += Vector3.up;
            }
            //X
            if (collisionPosition.x > transform.position.x)
            {
                direction += Vector3.left;
            }
            else if (collisionPosition.x < transform.position.x)
            {
                direction += Vector3.right;
            }
            //Z
            if (collisionPosition.z > transform.position.z)
            {
                direction += Vector3.back;
            }
            else if (collisionPosition.z < transform.position.z)
            {
                direction += Vector3.forward;
            }
        }
        Rigidbody otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            otherRigidbody.AddForce(direction * 20, ForceMode.VelocityChange);
        }
        direction = direction.normalized;
        Debug.DrawRay(transform.position, direction,Color.white,5f);
        //rigidbody.velocity = Vector3.Reflect(rigidbody.velocity.normalized, collision.GetContact(0).normal) * rigidbody.velocity.magnitude;
        rigidbody.velocity = rigidbody.velocity.magnitude * direction;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.MovePosition(transform.position + rigidbody.velocity * Time.deltaTime);
    }
}
