using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HookShot : WeaponNetwork
{


    [SerializeField] private GameObject handle;
    [SerializeField] private GameObject hook;
    [SerializeField] private float force = 200;

    public override void shoot()
    {

        shootServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc]
    public override void shootServerRpc(ulong clientID)
    {

        if (ammo.Value <= 0) return;

        ammo.Value--;

        hook.GetComponent<IDHolder>().setClientID(GetComponent<IDHolder>().getClientID());
        hook.transform.SetParent(null);
        hook.GetComponent<CapsuleCollider>().enabled = true;
        hook.GetComponent<Hook>().registerDropCallback(dropOnServer);
        hook.layer = LayerMask.NameToLayer("Damaging");

        Rigidbody rb = hook.AddComponent<Rigidbody>();
        Vector3 dir = transform.forward;
        rb.useGravity = false;
        rb.AddForce(dir * force);


        PlayerNetwork player = GameManager.instance.getPlayerWithID(clientID);
        player.GetComponent<PlayerMovement>().disableLookRotation();
        player.GetComponent<PlayerInput>().disableBattleControls();
    }

    public override void drop()
    {

    }

    [ServerRpc]
    public override void dropServerRpc()
    {

        dropOnServer();
    }

    public void dropOnServer()
    {

        PlayerNetwork player = GameManager.instance.getPlayerWithID(GetComponent<IDHolder>().getClientID());
        player.GetComponent<WeaponHolder>().dropWeapon();
        gameObject.AddComponent<Rigidbody>();
        transform.SetParent(null);

        gameObject.AddComponent<Destructor>().setDuration(5.0f);
        hook.AddComponent<Destructor>().setDuration(5.0f);
    }

    public override bool isEmpty()
    {

        return false;
    }

}
