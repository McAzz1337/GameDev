using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovCocktailMock : WeaponNetwork
{
    [SerializeField] private GameObject molotovPrefab;
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode();
        }
    }

    void Explode()
    {
        GameObject g = Instantiate(fireEffect, transform.position, transform.rotation);
        g.layer = LayerMask.NameToLayer("Damaging");
        SphereCollider sc = g.AddComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 2.0f;
        Destructor destructor = g.AddComponent<Destructor>();
        destructor.setDuration(5.0f);
        Destroy(gameObject);

    }
}
