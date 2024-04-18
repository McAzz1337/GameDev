using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Landmine : WeaponNetwork
{
    [SerializeField] private GameObject blastRadiusPrefab;
    public override void shoot()
    {
        transform.SetParent(null);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        Vector3 vector = 2*transform.forward;
        Quaternion rotationQuaternion = Quaternion.Euler(-90f, 0f, 0f);
        rb.transform.position += vector;
        rb.transform.rotation = rotationQuaternion;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")
            || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Explode(
                collision.contacts[0].point,
                GetComponent<Rigidbody>().velocity.normalized,
                collision.contacts[0].normal
            );
        }
    }

    private void Explode(Vector3 position, Vector3 direction, Vector3 normal)
    {
        GameObject g = Instantiate(blastRadiusPrefab, transform.position, transform.rotation);
        g.layer = LayerMask.NameToLayer("Damaging");
        g.GetComponent<NetworkObject>().Spawn(true);
        g.GetComponent<IDHolder>().setClientID(GetComponent<IDHolder>().getClientID());
        if (TryGetComponent<NetworkObject>(out NetworkObject n))
        {

            n.Despawn();
        }
        Destroy(gameObject);
    }
}
