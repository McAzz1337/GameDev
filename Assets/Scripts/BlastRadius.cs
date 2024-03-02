using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastRadius : MonoBehaviour
{
    [SerializeField] private float radius;
    void Start()
    {

        GetComponent<SphereCollider>().radius = radius;
    }

    void Update()
    {

    }


    void OnCollisionEnter(Collision collider)
    {

        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            // Take damage
        }
    }

    public void setRadius(float radius)
    {

        this.radius = radius;
        GetComponent<SphereCollider>().radius = radius;
    }
}
