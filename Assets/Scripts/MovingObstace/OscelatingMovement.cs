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
    private float elapsed;

    bool moving;

    Random random;

    // Start is called before the first frame update
    void Start()
    {

        moving = false;

        if (!IsHost) return;

        elapsed = 0.5f * travelDuration;
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
            startTime = Time.time;
        }
    }

    public void activate()
    {

        if (!IsHost) return;

        startTime = Time.time - elapsed;
        moving = true;
    }

    public void deactivate()
    {
        if (!IsHost) return;

        elapsed = Time.time - startTime;
        moving = false;
    }
}
