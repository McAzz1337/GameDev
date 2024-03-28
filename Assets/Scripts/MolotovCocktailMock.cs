using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEditor;
using UnityEngine;

public class MolotovCocktailMock : WeaponNetwork
{
    [SerializeField] private GameObject fireEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void shoot()
    {

        transform.SetParent(null);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();

        Vector3 throwDirection = transform.forward + transform.up;
        float throwForce = 10f;
        rb.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);
    }




    void OnCollisionEnter(Collision collision)
    {

        if (!IsHost) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            Explode();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {

            Rigidbody rb = GetComponent<Rigidbody>();
            float magnitude = rb.velocity.magnitude;
            Vector3 direction = rb.velocity.normalized;
            direction = Vector3.Reflect(direction, collision.contacts[0].normal);
            rb.velocity = direction * magnitude * 0.75f;
        }
    }

    void Explode()
    {

        GameObject g = Instantiate(fireEffect, transform.position, Quaternion.identity);
        g.GetComponent<NetworkObject>().Spawn(true);

        SphereCollider sc = g.AddComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 2.0f;

        g.AddComponent<Destructor>().setDuration(5.0f);

        g.GetComponent<IDHolder>().setClientID(GetComponent<IDHolder>().getClientID());

        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}
