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
    public delegate void DeathCallback(ulong clientID);
    public delegate void SuicideCallback();

    private DeathCallback onDeath;
    private SuicideCallback onSuicide;

    public void registerOnDeathCallback(DeathCallback callback)
    {

        onDeath += callback;
    }

    public void unregisterOnDeathCallback(DeathCallback callback)
    {

        onDeath -= callback;
    }



    public void registerOnSuicideCallback(SuicideCallback callback)
    {

        onSuicide += callback;
    }

    public void unregisterOnDeathCallback(SuicideCallback callback)
    {

        onSuicide -= callback;
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

        ulong shooterID = long.MaxValue;
        if (collider.gameObject.TryGetComponent<IDHolder>(out IDHolder i))
        {

            shooterID = i.getClientID();
        }

        collisionCheck(1 << collider.gameObject.layer, shooterID);
    }


    void OnTriggerStay(Collider collider)
    {

        ulong shooterID = long.MaxValue;
        if (collider.gameObject.TryGetComponent<IDHolder>(out IDHolder i))
        {

            shooterID = i.getClientID();
        }

        collisionCheck(1 << collider.gameObject.layer, shooterID);
    }

    private void collisionCheck(int layer, ulong shooterID)
    {

        bool takesDamage = IsOwner && !isDead() && (layer == LayerMask.GetMask("Damaging"));

        if (!takesDamage) return;

        takeDamageServerRpc(shooterID, NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc]
    public void takeDamageServerRpc(ulong shooterID, ulong hitClientID)
    {

        hp.Value--;

        if (isDead())
        {

            if (shooterID <= ulong.MaxValue)
            {

                if (shooterID != hitClientID)
                {

                    onDeath?.Invoke(shooterID);
                }
                else
                {

                    onSuicide?.Invoke();
                }
            }
            else
            {

                onSuicide?.Invoke();
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
