using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastRadius : MonoBehaviour
{
    [SerializeField] private float radius;
    void Start()
    {

        GetComponent<SphereCollider>().radius = radius;
        gameObject.AddComponent<Destructor>().setDuration(0.25f);
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
