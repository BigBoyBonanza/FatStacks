using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBossAI : MonoBehaviour
{
    public bool moveClockwise = false;
    public int speed;
    public Transform Helicopter;
    public Transform Player;
    public Bazooka[] bazookas;
    Coroutine TurnCoroutine;
    public Projectile rocket;
    public float fireRate;
    State currState = State.flyingForward;
    enum State
    {
        inActive,
        flyingForward,
        turning
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
                //Check if Player is behind helicopter.
                if (IsPlayerFacingHelicopter(true, 0.7f))
                {
                    TurnCoroutine = StartCoroutine("Turn180Degrees",1f);
                }
                break;
            case State.turning:
                break;
        }
    }

    IEnumerator Turn180Degrees(float rotTime)
    {
        currState = State.turning;
        float increment = 180 / rotTime;
        float progress = 0;
        Vector3 direction = (moveClockwise ? Vector3.up : Vector3.down);
        while (progress < 180)
        {
            Vector3 v = direction * increment * Time.deltaTime;
            progress += v.magnitude;
            Helicopter.transform.Rotate(v);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveClockwise = !moveClockwise;
        currState = State.flyingForward;
    }

    IEnumerator FireGuns()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            yield return new WaitUntil(() => IsPlayerFacingHelicopter(false,0.7f) == true);
            foreach(Bazooka bazooka in bazookas)
            {
                bazooka.fire1(new Ray());
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
