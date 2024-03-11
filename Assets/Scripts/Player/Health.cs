using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{

    [SerializeField]
    private NetworkVariable<int> hp =
        new NetworkVariable<int>(1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

    public delegate void DeathCalback();
    private DeathCalback onDeath;

    public void registerOnDeathCallback(DeathCalback callback)
    {

        onDeath += callback;
    }

    void OnCollisionEnter(Collision collision)
    {

        collisionCheck(1 << collision.gameObject.layer);
    }

    void OnCollisionStay(Collision collision)
    {

        collisionCheck(1 << collision.gameObject.layer);
    }


    void OnTriggerEnter(Collider collider)
    {

        collisionCheck(1 << collider.gameObject.layer);
    }


    void OnTriggerStay(Collider collider)
    {

        collisionCheck(1 << collider.gameObject.layer);
    }

    private void collisionCheck(int layer)
    {

        if (!IsOwner) return;

        if (isDead()) return;

        if (layer == LayerMask.GetMask("Damaging"))
        {

            takeDamageServerRpc();
        }
    }

    [ServerRpc]
    public void takeDamageServerRpc()
    {

        hp.Value--;

        if (isDead())
        {

            onDeath?.Invoke();
        }
    }

    public bool isDead()
    {

        return hp.Value <= 0;
    }

}
