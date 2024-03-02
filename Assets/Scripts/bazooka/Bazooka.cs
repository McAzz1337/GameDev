using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Weapon
{

    [SerializeField] private GameObject misslePrefab;
    [SerializeField] private Transform muzzle;


    void Start()
    {

        ammo = 5;
    }

    void Update()
    {

    }

    public override void shoot()
    {

        if (ammo <= 0) return;

        ammo--;

        Instantiate(misslePrefab, muzzle.position, muzzle.rotation);
    }
}
