using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBossAI : MonoBehaviour
{
    public bool moveClockwise = false;
    public float speed;
    public float rotSpeed;
    public float rangeToAlertFromBehind;
    public float rangeToAlertFromFront;
    public float rangeToLoseHelicopter;
    public Transform Helicopter;
    public Transform Player;
    Coroutine TurnCoroutine;
    bool canTurn = true;
    public Bazooka[] bazookas;
    public float fireRate;
    private bool fireLeft;
    [Range(0f, 1f)]
    public float fireCone;
    [Range(0f, 1f)]
    public float frontAlertCone;
    [Range(0f, 1f)]
    public float backAlertCone;
    public State currState = State.flyingForward;
    public enum State
    {
        inActive,
        flyingForward,
        flyingForwardAndAttacking,
    };

    private void Start()
    {
        currState = State.inActive;
        StartCoroutine("FireGuns");
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = 0;
        if (Player == null)
        {
            currState = State.inActive;
        }
        else
        {
            distance = Vector3.Distance(Helicopter.position, Player.position);
        }
        switch (currState)
        {
            case State.flyingForward:
                //Go forward
                transform.Rotate((moveClockwise ? Vector3.up : Vector3.down) * speed * Time.deltaTime);
                //Check if Player enters range of helicopter.
                if (IsPlayerFacingHelicopter(true, backAlertCone) && canTurn && distance < rangeToAlertFromBehind)
                {
                    TurnCoroutine = StartCoroutine("Turn180Degrees",1f);
                    currState = State.flyingForwardAndAttacking;
                }
                //Rotate back to normal orientation
                Quaternion dest = (moveClockwise) ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
                Helicopter.transform.localRotation = Quaternion.RotateTowards(Helicopter.localRotation, dest, rotSpeed * Time.deltaTime);
                if (IsPlayerFacingHelicopter(false, frontAlertCone) && distance < rangeToAlertFromFront)
                {
                    currState = State.flyingForwardAndAttacking;
                   
                }
                break;
            case State.flyingForwardAndAttacking:
                //Go forward
                transform.Rotate((moveClockwise ? Vector3.up : Vector3.down) * speed * Time.deltaTime);
                //Rotate towards player
                if (distance < rangeToLoseHelicopter)
                {
                    Helicopter.transform.rotation = Quaternion.RotateTowards(Helicopter.rotation, Quaternion.LookRotation(Helicopter.position - Player.position, Vector3.up), rotSpeed * Time.deltaTime);
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
            yield return new WaitUntil(() => IsPlayerFacingHelicopter(false,fireCone) == true);
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

    public bool IsPlayerFacingHelicopter(bool back, float threshold, bool checkForWall = true)
    {
        if (Player == null)
            return false;
        if (checkForWall)
        {
            bool wall = Physics.Linecast(Helicopter.position, Player.position, LayerMask.GetMask("Default", "InteractionSolid"));
            if (wall)
            {
                return false;
            }
        }
        Vector3 a = Helicopter.transform.forward;
        Vector3 b = (Helicopter.transform.position - Player.transform.position).normalized;
        float dot = Vector3.Dot(a, b);
        return (back && dot < -threshold) || (!back && dot > threshold);
    }
}
