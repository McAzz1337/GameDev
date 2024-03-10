using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Callbacks;
using UnityEngine;

public class Missle : MonoBehaviour
{


    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject blastRadiusPrefab;
    [SerializeField] private float force;
    void Start()
    {

        GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {

        int layer = 1 << collision.gameObject.layer;
        if (layer == LayerMask.GetMask("Wall") ||
            layer == LayerMask.GetMask("Ground") ||
            layer == LayerMask.GetMask("Player") ||
            layer == LayerMask.GetMask("Enemy"))
        {

            explode();

            expodeClientRpc(
                collision.contacts[0].point,
                GetComponent<Rigidbody>().velocity.normalized,
                collision.contacts[0].normal
            );
        }
    }

    public void explode()
    {

        GameObject g = Instantiate(blastRadiusPrefab, transform.position, transform.rotation);
        g.layer = LayerMask.NameToLayer("Damaging");
        Destroy(gameObject);

    }

    [ClientRpc]
    public void expodeClientRpc(Vector3 position, Vector3 direction, Vector3 normal)
    {

        Quaternion rot = Quaternion.LookRotation(Vector3.Cross(-direction, normal), normal);
        float angle = Vector3.Angle(-direction, normal);
        position -= direction * Mathf.Sin(angle) * 2;
        Instantiate(explosionPrefab, position, rot);
    }

}
