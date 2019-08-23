using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    public float initVelocity;
    public float baseSpeed;
    public float rotSpeed;
    private Quaternion targetDirection;
    public DroneChatter droneChatter;
    [HideInInspector]
    public Rigidbody rigidbody;
    public AudioSource source;
    public Box box;
    public State currState;

    public enum State
    {
        inActive,
        flyingForward,
        rotating
    };

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
        droneChatter.PlayRandomClip(DroneChatter.ChatterCategory.bumping,source);
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
        if (!box.Frozen)
        {
            Rigidbody otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody != null)
            {
                otherRigidbody.AddForce(direction * 10, ForceMode.VelocityChange);
            }
            direction = direction.normalized;
            rigidbody.velocity = rigidbody.velocity.magnitude * direction;
            
        }
        targetDirection = Quaternion.LookRotation(direction);
        currState = State.rotating;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currState)
        {
            case State.flyingForward:
                //Move forward
                rigidbody.MovePosition(transform.position + (baseSpeed * transform.forward) * Time.fixedDeltaTime);
                Debug.DrawRay(transform.position, transform.forward);
                break;
            case State.rotating:
                //Rotate
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetDirection, rotSpeed * Time.deltaTime);
                if(transform.rotation == targetDirection)
                {
                    currState = State.flyingForward;
                }
                break;
        }
        
    }
}
