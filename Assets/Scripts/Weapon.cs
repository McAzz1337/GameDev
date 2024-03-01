using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{


    [Header("Stats")]
    [SerializeField] private int ammo;

    void Start()
    {

    }

    void Update()
    {

    }

    public abstract void action();

}
