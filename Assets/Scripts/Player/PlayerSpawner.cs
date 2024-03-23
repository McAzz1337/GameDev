using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

using Random = System.Random;

public class PlayerSpawner : NetworkBehaviour
{

    public static PlayerSpawner instance = null;
    public SpawnPoint[] spawnPoints;
    private int spawnedPlayers;

    private Random random;
    void Awake()
    {

        instance = this;
        spawnedPlayers = 0;
        random = new Random();
    }

    public void SpawnPlayer(PlayerNetwork player)
    {

        if (!IsHost) return;

        if (spawnedPlayers >= GameManager.MAX_PLAYERS)
        {

            throw new System.Exception("Connected more Player than SpawnPoints in level");
        }

        int r;
        bool accepted = false;
        do
        {

            r = random.Next() % GameManager.MAX_PLAYERS;
            accepted = spawnPoints[r].acceptPlayer(player);
        } while (!accepted);


        spawnedPlayers++;
    }
}
