using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Hook : NetworkBehaviour
{

    public delegate void DropCallback();
    public DropCallback onHit;

    public void registerDropCallback(DropCallback callback)
    {

        onHit = callback;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (!IsHost) return;

        int layer = 1 << collision.gameObject.layer;
        Rigidbody rb = GetComponent<Rigidbody>();
        ulong clientID = GetComponent<IDHolder>().getClientID();

        if (layer == LayerMask.GetMask("Wall"))
        {

            rb.velocity = Vector3.zero;
            PlayerNetwork player = GameManager.instance.getPlayerWithID(clientID);
            player.GetComponent<PlayerMovement>().enableLookRotation();
            player.GetComponent<PlayerInput>().enableBattleControls();

            onHit.Invoke();
        }
        else if (layer == LayerMask.GetMask("Player"))
        {

            ulong hitClientID = collision.gameObject.GetComponent<IDHolder>().getClientID();

            if (clientID == hitClientID) return;

            Debug.Log("Hit from Hook");
            rb.velocity = Vector3.zero;
            PlayerNetwork player = GameManager.instance.getPlayerWithID(clientID);
            player.GetComponent<PlayerMovement>().enableLookRotation();
            player.GetComponent<PlayerInput>().enableBattleControls();

            player.transform.position = collision.transform.position;

            onHit.Invoke();
        }
    }
}
