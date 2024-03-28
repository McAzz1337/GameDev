using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{

    public static RoundManager instance = null;

    private List<GameObject> toActivate = new List<GameObject>();

    void Awake()
    {

        instance = this;

    }



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

    }


    void Start()
    {

        startRound();
    }

    void startRound()
    {

        if (!IsHost) return;

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {

            NetworkClient client = NetworkManager.Singleton.ConnectedClients[(ulong)i];
            PlayerNetwork player = client.PlayerObject.GetComponent<PlayerNetwork>();
            Debug.Log("null");
            player.enableBattleControls();

            PlayerSpawner.instance.SpawnPlayer(player);
        }


        foreach (GameObject g in toActivate)
        {

            if (g.TryGetComponent<WeaponSpawnerNetwork>(out WeaponSpawnerNetwork w))
            {

                w.acitvate();
            }
        }
    }

    public void endRound()
    {

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {

            NetworkClient client = NetworkManager.Singleton.ConnectedClients[(ulong)i];
            PlayerNetwork player = client.PlayerObject.GetComponent<PlayerNetwork>();
            player.disableBattleControls();
        }

    }



    public void addGameObjectToActivate(GameObject g)
    {

        toActivate.Add(g);
    }

}
