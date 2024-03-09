using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    [SerializeField] private GameObject[] toActivate;
    public static GameManager instance;
    [SerializeField] private Transform[] spawnTransforms;
    [SerializeField] private PlayerNetwork[] connectedPlayers;

    void Awake()
    {

        instance = this;
        connectedPlayers = new PlayerNetwork[spawnTransforms.Length];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void playerConnected(PlayerNetwork player)
    {


        for (int i = 0; i < spawnTransforms.Length; i++)
        {

            if (connectedPlayers[i] == null)
            {

                player.transform.position = spawnTransforms[i].position;
                connectedPlayers[i] = player;

                break;
            }
        }
    }

    public void acitvate()
    {

        foreach (GameObject g in toActivate)
        {

            g.SetActive(true);
        }
    }

}
