using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{


    [Header("Stats")]
    [SerializeField] protected int ammo;

    void Start()
    {

    }

    void Update()
    {

    }

    public abstract void shoot();

    public bool isEmpty()
    {

        return ammo <= 0;
    }

}
