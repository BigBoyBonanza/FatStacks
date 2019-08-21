using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBossAI : MonoBehaviour
{
    public bool moveClockwise = false;
    public float speed;
    public float range;
    public Transform Helicopter;
    public Transform Player;
    Coroutine TurnCoroutine;
    bool canTurn = true;
    public Bazooka[] bazookas;
    public Projectile rocket;
    public float fireRate;
    private bool fireLeft;
    State currState = State.flyingForward;
    enum State
    {
        inActive,
        flyingForward,
        flyingForwardAndAttacking,
    };

    private void Start()
    {
        StartCoroutine("FireGuns");
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case State.flyingForward:
                //Go forward
                transform.Rotate((moveClockwise ? Vector3.up : Vector3.down) * speed * Time.deltaTime);
                //Check if Player enters range of helicopter.
                if (IsPlayerFacingHelicopter(true, 0.8f) && canTurn)
                {
                    TurnCoroutine = StartCoroutine("Turn180Degrees",1f);
                    currState = State.flyingForwardAndAttacking;
                }
                //Rotate back to normal orientation
                Quaternion dest = (moveClockwise) ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
                Helicopter.transform.localRotation = Quaternion.Slerp(Helicopter.localRotation, dest, 0.01f);
                if (IsPlayerFacingHelicopter(false, 0.6f) && Vector3.Distance(Helicopter.position,Player.position) < range)
                {
                    currState = State.flyingForwardAndAttacking;
                   
                }
                break;
            case State.flyingForwardAndAttacking:
                //Go forward
                transform.Rotate((moveClockwise ? Vector3.up : Vector3.down) * speed * Time.deltaTime);
                //Rotate towards player
                if (Vector3.Distance(Helicopter.position, Player.position) < range)
                {
                    Helicopter.transform.rotation = Quaternion.Slerp(Helicopter.rotation, Quaternion.LookRotation(Helicopter.position - Player.position, Vector3.up), 0.01f);
                }
                else
                {
                    currState = State.flyingForward;
                }
                break;
        }
    }
    
    IEnumerator Turn180Degrees(float rotTime)
    {
        moveClockwise = !moveClockwise;
        canTurn = false;
        yield return new WaitForSeconds(rotTime);
        canTurn = true;
        //currState = State.turning;
        //float increment = 180 / rotTime;
        //float progress = 0;
        //Vector3 direction = (moveClockwise ? Vector3.up : Vector3.down);
        //while (progress < 180)
        //{
        //    Vector3 v = direction * increment * Time.deltaTime;
        //    progress += v.magnitude;
        //    Helicopter.transform.Rotate(v);
        //    yield return new WaitForSeconds(Time.deltaTime);
        //}
        //moveClockwise = !moveClockwise;
        //currState = State.flyingForward;
    }
    
    IEnumerator FireGuns()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            yield return new WaitUntil(() => IsPlayerFacingHelicopter(false,0.9f) == true);
            fireLeft = !fireLeft;
            if (fireLeft)
            {
                bazookas[0].fire1(new Ray());
            }
            else
            {
                bazookas[1].fire1(new Ray());
            }
        }
    }

    bool IsPlayerFacingHelicopter(bool back, float threshold)
    {
        Vector3 a = Helicopter.transform.forward;
        Vector3 b = (Helicopter.transform.position - Player.transform.position).normalized;
        float dot = Vector3.Dot(a, b);
        return (back && dot < -threshold) || (!back && dot > threshold);
    }
}
