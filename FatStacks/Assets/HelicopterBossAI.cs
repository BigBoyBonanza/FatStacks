using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterBossAI : MonoBehaviour
{
    public bool moveClockwise = false;
    public int speed;
    public Transform Helicopter;
    public Transform Player;
    Coroutine TurnCoroutine;
    State currState = State.flyingForward;
    enum State
    {
        inActive,
        flyingForward,
        turning
    };

    // Start is called before the first frame update
    void Start()
    {
        
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
                Vector3 a = Helicopter.transform.forward;
                Vector3 b = (Helicopter.transform.position - Player.transform.position).normalized;
                float dot = Vector3.Dot(a, b);
                if(dot < -0.7f)
                {
                    StartCoroutine("Turn180Degrees",1f);
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
        while(progress < 180)
        {
            Vector3 v = Vector3.down * increment * Time.deltaTime;
            progress += v.magnitude;
            Helicopter.transform.Rotate(v);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        moveClockwise = !moveClockwise;
        currState = State.flyingForward;
    }
}
