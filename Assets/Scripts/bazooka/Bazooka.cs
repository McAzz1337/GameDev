using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class Bazooka : WeaponNetwork
{


    void Start()
    {
        init();

    }

    void Update()
    {

    }

    public override void shoot()
    {

        if (ammo.Value <= 0) return;

        shootServerRpc();

        GameObject muzzleFlash = Instantiate(stats.muzzleFlashPrefab, muzzle.position, muzzle.rotation);
        Destructor d = muzzleFlash.AddComponent<Destructor>();
        d.setDuration(0.25f);
        d.setDestructableByClient(true);
    }

    [ServerRpc]
    public override void shootServerRpc()
    {

        Vector3 direction = muzzle.position - transform.position;
        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, direction.magnitude, LayerMask.GetMask("Wall")))
        {

            GameObject g = (GameObject)Instantiate(
                Resources.Load("WeaponsNetwork/BlastRadius"),
                hit.transform.position,
                Quaternion.identity);

            g.GetComponent<NetworkObject>().Spawn(true);
            g.layer = LayerMask.NameToLayer("Damaging");

            Quaternion rot = Quaternion.LookRotation(Vector3.Cross(-direction, hit.normal), hit.normal);
            float angle = Vector3.Angle(-direction, hit.normal);
            Vector3 position = hit.transform.position - direction * Mathf.Sin(angle) * 2;
        }
        else
        {
            GameObject g = Instantiate(stats.projectilePrefab, muzzle.position, muzzle.rotation);
            g.GetComponent<NetworkObject>().Spawn();
            ammo.Value--;
        }
    }


}
