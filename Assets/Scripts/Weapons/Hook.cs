using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Hook : Projectile
{

    public delegate void DropCallback();
    public DropCallback onHit;

    [SerializeField] private float force = 1000;

    public void registerDropCallback(DropCallback callback)
    {

        onHit = callback;
    }

    public void shoot()
    {

        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(transform.up * force);
        rb.useGravity = false;


    }

    public void setLayer(ulong shooterID)
    {

        setLayerClientRpc(shooterID);

        if (shooterID != NetworkManager.Singleton.LocalClientId)
        {

            gameObject.layer = LayerMask.NameToLayer("Damaging");
        }
    }

    [ClientRpc]
    public void setLayerClientRpc(ulong shooterID)
    {

        if (shooterID == NetworkManager.Singleton.LocalClientId) return;

        gameObject.layer = LayerMask.NameToLayer("Damaging");
    }

    void OnCollisionEnter(Collision collision)
    {

        if (!IsHost) return;

        int layer = 1 << collision.gameObject.layer;
        Rigidbody rb = GetComponent<Rigidbody>();
        ulong clientID = GetComponent<IDHolder>().getClientID();

        if (layer == LayerMask.GetMask("Player"))
        {

            ulong hitClientID = collision.gameObject.GetComponent<IDHolder>().getClientID();

            if (clientID == hitClientID) return;

            Debug.Log("hit from Hook: " + hitClientID);

            rb.velocity = Vector3.zero;
            PlayerNetwork player = GameManager.instance.getPlayerWithID(clientID);
            player.GetComponent<PlayerMovement>().enableLookRotation();
            player.GetComponent<PlayerInput>().enableBattleControls();

            player.transform.position = collision.transform.position;

            onHit.Invoke();
        }
        else
        {

            rb.velocity = Vector3.zero;
            PlayerNetwork player = GameManager.instance.getPlayerWithID(clientID);
            player.GetComponent<PlayerMovement>().enableLookRotation();
            player.GetComponent<PlayerInput>().enableBattleControls();

            onHit.Invoke();
        }
    }
}
