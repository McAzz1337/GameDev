using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMock : Weapon
{

    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform muzzle;

    void Start()
    {

        ammo = 1000;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void shoot()
    {

        Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
    }

}
