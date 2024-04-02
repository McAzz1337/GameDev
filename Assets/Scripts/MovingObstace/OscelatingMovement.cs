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
    [SerializeField] private BoxCollider killZoneTop;
    [SerializeField] private BoxCollider killZoneBottom;
    private BoxCollider boxCollider;
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

        boxCollider = GetComponent<BoxCollider>();
        moving = false;
        disableKillZones();

        if (!IsHost) return;

        elapsed = 0.5f * travelDuration;
        random = new Random();
        int r = random.Next();
        start = (r & 0b1) == 1 ? transformA : transformB;
        target = (r & 0b1) == 1 ? transformB : transformA;

        if (!IsHost) return;

        calculatePosition();
    }

    void Update()
    {

        if (!IsHost || !moving) return;

        float t = calculatePosition();
        if (Mathf.Abs(transform.position.z - target.position.z) < boxCollider.size.z + 1.0f)
        {

            if (target.position.z > transform.position.z)
            {

                killZoneTop.enabled = true;
            }
            else if (target.position.z < transform.position.z)
            {

                killZoneBottom.enabled = true;
            }
        }

        if (t >= 1.0f)
        {

            Transform temp = start;
            start = target;
            target = temp;
            startTime = Time.time;
            disableKillZones();
        }
    }

    private float calculatePosition()
    {

        float t = (Time.time - startTime) / travelDuration;

        float z = Mathf.SmoothStep(start.position.z, target.position.z, t);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        return t;
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
        disableKillZones();
    }

    public void disableKillZones()
    {

        killZoneTop.enabled = false;
        killZoneBottom.enabled = false;
    }
}
