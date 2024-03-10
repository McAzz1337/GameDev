using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Missle : NetworkBehaviour
{


    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject blastRadiusPrefab;
    [SerializeField] private float force;
    void Start()
    {

        if (!IsHost) return;

        GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {

        if (!IsHost) return;

        int layer = 1 << collision.gameObject.layer;
        if (layer == LayerMask.GetMask("Wall") ||
            layer == LayerMask.GetMask("Ground") ||
            layer == LayerMask.GetMask("Player") ||
            layer == LayerMask.GetMask("Enemy"))
        {

            explode(
                collision.contacts[0].point,
                GetComponent<Rigidbody>().velocity.normalized,
                collision.contacts[0].normal
            );

        }
    }

    public void explode(Vector3 position, Vector3 direction, Vector3 normal)
    {

        GameObject g = Instantiate(blastRadiusPrefab, transform.position, transform.rotation);
        g.layer = LayerMask.NameToLayer("Damaging");
        g.GetComponent<NetworkObject>().Spawn(true);


        Quaternion rot = Quaternion.LookRotation(Vector3.Cross(-direction, normal), normal);
        float angle = Vector3.Angle(-direction, normal);
        position -= direction * Mathf.Sin(angle) * 2;

        g = Instantiate(explosionPrefab, position, rot);
        g.GetComponent<NetworkObject>().Spawn(true);


        Destroy(gameObject);

    }


}
