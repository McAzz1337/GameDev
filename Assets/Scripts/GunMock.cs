using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMock : Weapon
{


    void Start()
    {

    }

    void Update()
    {

    }

    public override void shoot()
    {

        Instantiate(stats.projectilePrefab, muzzle.position, muzzle.rotation);
        GameObject muzzleFlash = Instantiate(stats.muzzleFlashPrefab, muzzle.position, muzzle.rotation);
        muzzleFlash.transform.SetParent(muzzle);
        muzzleFlash.AddComponent<Destructor>().setDuration(0.25f);
    }

}
