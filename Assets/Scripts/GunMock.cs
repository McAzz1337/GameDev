using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMock : Weapon
{

    [SerializeField] private GameObject bulletPrefab;

    void Start()
    {

        ammo = 1;
    }

    void Update()
    {

    }

    public override void shoot()
    {

        Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
    }

}
