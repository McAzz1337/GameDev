using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BlastRadius : NetworkBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private GameObject explosionPrefab;

    void Start()
    {

        if (!IsHost) return;

        GetComponent<SphereCollider>().radius = radius;

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

}
