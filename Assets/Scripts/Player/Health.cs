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

    // make DeathCallback take id of shooting player
    public delegate void DeathCalback(ulong clientID);
    private DeathCalback onDeath;

    public void registerOnDeathCallback(DeathCalback callback)
    {

        onDeath += callback;
    }

    public void unregisterOnDeathCallback(DeathCalback callback)
    {

        onDeath -= callback;
    }


    void OnCollisionEnter(Collision collision)
    {

        ulong clientID = long.MaxValue;
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile p))
        {

            clientID = p.getClientID();
        }
        else if (collision.gameObject.TryGetComponent<BlastRadius>(out BlastRadius b))
        {

            clientID = b.getClientID();
        }

        collisionCheck(1 << collision.gameObject.layer, clientID);
    }

    void OnCollisionStay(Collision collision)
    {

        ulong clientID = long.MaxValue;
        if (collision.gameObject.TryGetComponent<Projectile>(out Projectile p))
        {

            clientID = p.getClientID();
        }
        else if (collision.gameObject.TryGetComponent<BlastRadius>(out BlastRadius b))
        {

            clientID = b.getClientID();
        }

        collisionCheck(1 << collision.gameObject.layer, clientID);
    }


    void OnTriggerEnter(Collider collider)
    {

        ulong clientID = long.MaxValue;
        if (collider.gameObject.TryGetComponent<Projectile>(out Projectile p))
        {

            collisionCheck(1 << collider.gameObject.layer, p.getClientID());
        }
        else if (collider.gameObject.TryGetComponent<BlastRadius>(out BlastRadius b))
        {

            clientID = b.getClientID();
        }

        collisionCheck(1 << collider.gameObject.layer, clientID);
    }


    void OnTriggerStay(Collider collider)
    {

        ulong clientID = long.MaxValue;
        if (collider.gameObject.TryGetComponent<Projectile>(out Projectile p))
        {

            collisionCheck(1 << collider.gameObject.layer, p.getClientID());
        }
        else if (collider.gameObject.TryGetComponent<BlastRadius>(out BlastRadius b))
        {

            clientID = b.getClientID();
        }

        collisionCheck(1 << collider.gameObject.layer, clientID);
    }

    private void collisionCheck(int layer, ulong clientID)
    {

        if (!IsOwner) return;

        if (isDead()) return;

        if (layer == LayerMask.GetMask("Damaging"))
        {
            takeDamageServerRpc(clientID);
        }
    }

    [ServerRpc]
    public void takeDamageServerRpc(ulong clientID)
    {

        hp.Value--;

        if (isDead())
        {

            onDeath?.Invoke(clientID);
        }
    }

    public bool isDead()
    {

        return hp.Value <= 0;
    }

}
