using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{

    public static RoundManager instance = null;

    private List<GameObject> toActivate = new List<GameObject>();

    private int spawnedClients = 0;
    void Awake()
    {

        instance = this;

    }



    public override void OnNetworkSpawn()
    {


        Debug.Log("Spawned on network id:" + NetworkManager.Singleton.LocalClientId);
        NetworkManager.SceneManager.OnSceneEvent += onSceneEvent;

        base.OnNetworkSpawn();

    }

    private void onSceneEvent(SceneEvent e)
    {

        if (e.SceneEventType == SceneEventType.LoadEventCompleted)
        {

            if (IsHost)
            {

                spawnedClients++;

                if (spawnedClients == GameManager.instance.getPlayerCount())
                {

                    startRound();
                }
            }
            else
            {

                spawnClientServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void spawnClientServerRpc()
    {

        spawnedClients++;

        if (spawnedClients == GameManager.instance.getPlayerCount())
        {

            startRound();
        }
    }


    void Start()
    {

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
