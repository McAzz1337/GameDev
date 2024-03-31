using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{

    public static RoundManager instance = null;

    private List<GameObject> toActivate = new List<GameObject>();

    [SerializeField]
    private int deadPlayers = 0;
    private int spawnedClients = 0;
    void Awake()
    {

        instance = this;
    }



    public override void OnNetworkSpawn()
    {


        NetworkManager.SceneManager.OnSceneEvent += onSceneEvent;

        base.OnNetworkSpawn();

    }

    private void onSceneEvent(SceneEvent e)
    {

        if (e.SceneEventType == SceneEventType.LoadEventCompleted)
        {

            if (IsHost)
            {

                countSpawnedClients();
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

        countSpawnedClients();
    }

    private void countSpawnedClients()
    {

        if (!IsHost) return;

        spawnedClients++;

        if (spawnedClients == GameManager.instance.getPlayerCount())
        {

            startRound();
        }

    }

    public void acknowledgeSuicide()
    {

        ackgnowledgeDeath();
    }

    public void ackgnowledgeDeath(ulong shooterID = ulong.MaxValue)
    {

        Debug.Log("Shot by: " + shooterID);
        deadPlayers++;
        if (playersAlive() <= 1)
        {

            endRound();
        }
    }

    public int playersAlive()
    {

        return NetworkManager.Singleton.ConnectedClients.Count - deadPlayers;
    }

    void startRound()
    {

        if (!IsHost) return;

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {

            NetworkClient client = NetworkManager.Singleton.ConnectedClients[(ulong)i];
            PlayerNetwork player = client.PlayerObject.GetComponent<PlayerNetwork>();
            Debug.Log("null");
            player.GetComponent<PlayerInput>().enableBattleControls();
            player.enableRenderer();

            Health h = player.GetComponent<Health>();
            h.resetHealth();
            h.registerOnDeathCallback(ackgnowledgeDeath);
            h.registerOnSuicideCallback(acknowledgeSuicide);

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

        Debug.Log("END ROUND");
        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {

            NetworkClient client = NetworkManager.Singleton.ConnectedClients[(ulong)i];
            PlayerNetwork player = client.PlayerObject.GetComponent<PlayerNetwork>();
            player.GetComponent<PlayerInput>().disableBattleControls();
        }

    }



    public void addGameObjectToActivate(GameObject g)
    {

        toActivate.Add(g);
    }



}
