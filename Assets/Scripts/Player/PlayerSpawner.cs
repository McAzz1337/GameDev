using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public int amount;
    public Transform[] spawnPoints;
    private float spawnRadius = 1f;



    public void SpawnPlayer()
    {

        if(amount > spawnPoints.Length){
            throw new System.Exception("Not enough spawnPoints available for player");
        }

        for(int i = 0; i < amount; i++)
        {
            Transform spawnPoint = GetRandomSpawnPoint();

            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        }

    }

    private Transform GetRandomSpawnPoint()
    {
        System.Random rng = new System.Random();
        int n = spawnPoints.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Transform value = spawnPoints[k];
            spawnPoints[k] = spawnPoints[n];
            spawnPoints[n] = value;
        }

        // Get all Player with LayerMask Player
        int layerToFind = LayerMask.NameToLayer("Player");
        GameObject[] objectsWithLayer = GameObject.FindObjectsOfType<GameObject>().Where(go => go.layer == layerToFind).ToArray();

        // Check the spawn positions to ensure that the distance between players is large enough
        foreach (Transform spawnPoint in spawnPoints)
        {
            bool validSpawnPoint = true;
            foreach (GameObject player in objectsWithLayer)
            {
                float distance = Vector3.Distance(spawnPoint.position, player.transform.position);
                if (distance < spawnRadius)
                {
                    //invalid if distance is to short
                    validSpawnPoint = false;
                    break;
                }
            }
            
            if (validSpawnPoint)
            {
                return spawnPoint;
            }
        }


        throw new System.Exception("No valid spawnPoints for SpawnPlayer. Make sure that the Spawnpoints have enough distance for eachother.");

    }

    public void setAmount(int amount)
    {
        this.amount = amount;
    }

}
