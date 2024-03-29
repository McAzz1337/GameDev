using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    bool moving;

    Random random;

    // Start is called before the first frame update
    void Start()
    {

        moving = false;

        if (!IsHost) return;

        random = new Random();
        int r = random.Next();
        start = transform;
        target = (r & 0b1) == 1 ? transformB : transformA;
    }

    void Update()
    {

        if (!IsHost || !moving) return;

        float t = (Time.time - startTime) / actualDuration;

        float z = Mathf.SmoothStep(start.position.z, target.position.z, t);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        if (t >= 1.0f)
        {

            if (start == transform)
            {

                start = target == transformA ? transformB : transformA;
            }

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

        startTime = Time.time;
        moving = true;

        float percentageAlreadyTraveled =
            (target.position - transform.position).magnitude / (transformA.position - transformB.position).magnitude;

        percentageAlreadyTraveled = Mathf.Max(1.0f - percentageAlreadyTraveled, 0.000001f);
        actualDuration = percentageAlreadyTraveled * travelDuration;
    }

    public void deactivate()
    {

        moving = false;
    }
}
