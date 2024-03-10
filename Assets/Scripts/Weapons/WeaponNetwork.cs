using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;

public class WeaponNetwork : NetworkBehaviour
{

    [SerializeField] private Transform muzzle;
    [SerializeField] private WeaponStats stats;
    [SerializeField] private EWeapon identifier;
    [SerializeField]
    private NetworkVariable<int> ammo =
                                new NetworkVariable<int>(
                                    1,
                                    NetworkVariableReadPermission.Everyone,
                                    NetworkVariableWritePermission.Server
                                );

    void Start()
    {

        ammo.Value = stats.ammo;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    protected void init()
    {


        GetComponentInChildren<MeshCollider>().enabled = false;
    }

    void Update()
    {

    }

    public virtual void shoot()
    {

        shootServerRpc();

        GameObject muzzleFlash = Instantiate(stats.muzzleFlashPrefab, muzzle.position, muzzle.rotation);
        Destructor d = muzzleFlash.AddComponent<Destructor>();
        d.setDuration(0.25f);
        d.setDestructableByClient(true);
    }

    [ServerRpc]
    public void shootServerRpc()
    {

        if (ammo.Value <= 0) return;

        GameObject g = Instantiate(stats.projectilePrefab, muzzle.position, muzzle.rotation);
        g.GetComponent<NetworkObject>().Spawn();
        ammo.Value--;

    }



    public virtual void drop()
    {

        dropServerRpc();
    }

    [ServerRpc]
    public void dropServerRpc()
    {

        transform.SetParent(null);
        gameObject.AddComponent<Rigidbody>();
        GetComponent<CapsuleCollider>().enabled = true;
        gameObject.AddComponent<Destructor>().setDuration(3.0f);
    }


    public bool isEmpty()
    {

        return ammo.Value <= 0;
    }


    public EWeapon getIdentifier()
    {

        return identifier;
    }

}
