using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Weapon
{

    [SerializeField] private GameObject misslePrefab;
    [SerializeField] private GameObject muzzleflashPrefab;


    void Start()
    {

        ammo = 5;
    }

    void Update()
    {

    }

    public override void shoot()
    {

        if (isEmpty()) return;

        ammo--;

        Instantiate(misslePrefab, muzzle.position, muzzle.rotation);
        GameObject g = Instantiate(muzzleflashPrefab, muzzle.position, muzzle.rotation);
        g.transform.SetParent(muzzle);
        g.AddComponent<Destructor>().setDuration(0.25f);
    }
}
