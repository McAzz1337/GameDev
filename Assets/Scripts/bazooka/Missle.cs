using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{


    [SerializeField] private GameObject blastRadiusPrefab;
    [SerializeField] private float force;
    void Start()
    {

        GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collider)
    {

        int layer = 1 << collider.gameObject.layer;
        if (layer == LayerMask.GetMask("Wall") ||
            layer == LayerMask.GetMask("Ground") ||
            layer == LayerMask.GetMask("Player"))
        {

            explode();
        }
    }

    public void explode()
    {

        Instantiate(blastRadiusPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
