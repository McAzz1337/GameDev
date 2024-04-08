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

        shootServerRpc(GetComponent<IDHolder>().getClientID());
    }

    [ServerRpc]
    public override void shootServerRpc(ulong clientID)
    {

        if (ammo.Value <= 0) return;

        ammo.Value--;

        hook.GetComponent<IDHolder>().setClientID(clientID);
        hook.transform.SetParent(null);
        hook.GetComponent<CapsuleCollider>().enabled = true;

        Hook hookObject = hook.GetComponent<Hook>();
        hookObject.registerDropCallback(dropOnServer);
        hookObject.setLayer(clientID);
        hookObject.shoot();


        PlayerNetwork player = GameManager.instance.getPlayerWithID(clientID);
        player.GetComponent<PlayerMovement>().disableLookRotation();
        player.GetComponent<PlayerInput>().disableBattleControls();
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
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

        transform.SetParent(null);
        PlayerNetwork player = GameManager.instance.getPlayerWithID(GetComponent<IDHolder>().getClientID());
        player.GetComponent<WeaponHolder>().dropWeapon();
        gameObject.AddComponent<Rigidbody>();

        gameObject.AddComponent<Destructor>().setDuration(3.0f);
        hook.AddComponent<Destructor>().setDuration(3.0f);
    }

    public override bool isEmpty()
    {

        return false;
    }

}
