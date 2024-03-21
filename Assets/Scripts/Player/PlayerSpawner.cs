using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

using Random = System.Random;

public class PlayerSpawner : NetworkBehaviour
{
    public int amount;
    public SpawnPoint[] spawnPoints;
    private int spawnedPlayers;
    private float spawnRadius = 1f;

    private Random random;
    void Awake()
    {

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
        } while (accepted);


        spawnedPlayers++;
    }
}
