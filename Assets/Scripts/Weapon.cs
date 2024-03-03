using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{


    [SerializeField] protected Transform muzzle;

    [Header("Stats")]
    [SerializeField] protected int ammo;

    void Start()
    {

    }

    void Update()
    {

    }

    public abstract void shoot();

    public void drop()
    {

        Rigidbody rb;
        if (!TryGetComponent(out rb))
        {

            rb = gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<CapsuleCollider>();
            transform.SetParent(null);
            gameObject.AddComponent<Destructor>().setDuration(5.0f);
        }



    }

    public bool isEmpty()
    {

        return ammo <= 0;
    }

}
