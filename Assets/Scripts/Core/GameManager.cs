using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    [SerializeField] private GameObject[] toActivate;
    public static GameManager instance;
    [SerializeField] private Transform spawnTransform;

    void Awake()
    {

        instance = this;
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

        player.transform.position = spawnTransform.position;
    }

    public void acitvate()
    {

        foreach (GameObject g in toActivate)
        {

            g.SetActive(true);
        }
    }

}
