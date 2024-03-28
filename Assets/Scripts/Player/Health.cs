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
        if (collision.gameObject.TryGetComponent<IDHolder>(out IDHolder i))
        {

            clientID = i.getClientID();
        }

        collisionCheck(1 << collision.gameObject.layer, clientID);
    }

    void OnCollisionStay(Collision collision)
    {

        ulong clientID = long.MaxValue;
        if (collision.gameObject.TryGetComponent<IDHolder>(out IDHolder i))
        {

            clientID = i.getClientID();
        }

        collisionCheck(1 << collision.gameObject.layer, clientID);
    }


    void OnTriggerEnter(Collider collider)
    {

        ulong clientID = long.MaxValue;
        if (collider.gameObject.TryGetComponent<IDHolder>(out IDHolder i))
        {

            clientID = i.getClientID();
        }

        collisionCheck(1 << collider.gameObject.layer, clientID);
    }


    void OnTriggerStay(Collider collider)
    {

        ulong clientID = long.MaxValue;
        if (collider.gameObject.TryGetComponent<IDHolder>(out IDHolder i))
        {

            clientID = i.getClientID();
        }

        collisionCheck(1 << collider.gameObject.layer, clientID);
    }

    private void collisionCheck(int layer, ulong clientID)
    {

        if (clientID < (ulong)GameManager.MAX_PLAYERS)
        {
            Debug.Log("Collision with clientID: " + clientID);
            Debug.Log("IsOwner: " + IsOwner);
            Debug.Log("dead: " + isDead());
            Debug.Log("layer is damaging: " + (layer == LayerMask.NameToLayer("Damaging")));
        }

        if (!IsOwner || isDead() || layer != LayerMask.NameToLayer("Damaging")) return;

        Debug.Log("Hit by Damaging from client: " + clientID);

        takeDamageServerRpc(clientID);
    }

    [ServerRpc]
    public void takeDamageServerRpc(ulong clientID)
    {

        hp.Value--;

        if (isDead())
        {

            if (clientID != ulong.MaxValue)
            {

                onDeath?.Invoke(clientID);
            }


            PlayerNetwork player = gameObject.GetComponent<PlayerNetwork>();
            player.disableBattleControls();
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mr.enabled = false;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            Transform cam = Camera.main.transform;
            player.transform.position = cam.position + new Vector3(0.0f, 0.0f, cam.forward.z * -10f);
        }
    }

    public bool isDead()
    {

        return hp.Value <= 0;
    }

}
