using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;

public class WeaponNetwork : NetworkBehaviour
{

    [SerializeField] protected Transform muzzle;
    [SerializeField] protected WeaponStats stats;
    [SerializeField] private EWeapon identifier;
    [SerializeField]
    protected NetworkVariable<int> ammo =
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



        if (ammo.Value <= 0) return;

        ulong clientID = NetworkManager.Singleton.LocalClientId;
        shootServerRpc(clientID);

        GameObject muzzleFlash = Instantiate(stats.muzzleFlashPrefab, muzzle.position, muzzle.rotation);
        Destructor d = muzzleFlash.AddComponent<Destructor>();
        d.setDuration(0.25f);
        d.setDestructableByClient(true);
    }

    [ServerRpc]
    public virtual void shootServerRpc(ulong clientID)
    {


        GameObject g = Instantiate(stats.projectilePrefab, muzzle.position, muzzle.rotation);
        g.GetComponent<NetworkObject>().Spawn();
        g.GetComponent<Projectile>().setClientID(clientID);
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
