using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Unity.Netcode;
using UnityEngine;

using Random = System.Random;

public class OscelatingMovement : NetworkBehaviour
{

    [SerializeField] private Transform transformA;
    [SerializeField] private Transform transformB;
    private float startTime;
    private Transform target;
    private Transform start;
    [SerializeField] private float travelDuration;
    private float actualDuration;
    private float elapsed;

    bool moving;

    Random random;

    // Start is called before the first frame update
    void Start()
    {

        elapsed = 0.5f * travelDuration;
        moving = false;

        if (!IsHost) return;

        random = new Random();
        int r = random.Next();
        start = (r & 0b1) == 1 ? transformA : transformB;
        target = (r & 0b1) == 1 ? transformB : transformA;
    }

    void Update()
    {

        if (!IsHost || !moving) return;

        float t = (Time.time - startTime) / travelDuration;

        float z = Mathf.SmoothStep(start.position.z, target.position.z, t);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        if (t >= 1.0f)
        {

            Transform temp = start;
            start = target;
            target = temp;
            actualDuration = travelDuration;
            startTime = Time.time;
        }
    }

    public void activate()
    {

        if (!IsHost) return;

        startTime = Time.time - elapsed;
        moving = true;

        float percentageAlreadyTraveled =
            Mathf.Abs(target.position.z - transform.position.z) / Mathf.Abs(transformA.position.z - transformB.position.z);

        percentageAlreadyTraveled = Mathf.Max(1.0f - percentageAlreadyTraveled, 0.000001f);
        actualDuration = percentageAlreadyTraveled * travelDuration;
    }

    public void deactivate()
    {
        elapsed = Time.time - startTime;
        moving = false;
    }
}
