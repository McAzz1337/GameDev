using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BlastRadius : NetworkBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private GameObject explosionPrefab;

    private ulong clientID;

    void Start()
    {

        if (!IsHost) return;

        GetComponent<SphereCollider>().radius = radius;
        gameObject.AddComponent<Destructor>().setDuration(0.25f);

        GameObject g = Instantiate(explosionPrefab, transform.position, transform.rotation);
        g.GetComponent<NetworkObject>().Spawn(true);
    }

    void Update()
    {

    }

    public void setRadius(float radius)
    {

        this.radius = radius;
        GetComponent<SphereCollider>().radius = radius;
    }

    public void setClientID(ulong clientID)
    {

        this.clientID = clientID;
    }

    public ulong getClientID()
    {

        return clientID;
    }
}
