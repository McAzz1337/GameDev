using TMPro;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static Health;
using System.Collections.Generic;

public class PointManager : NetworkBehaviour
{
    public static PointManager instance = null;

    // Class to Save Points of one Player.
    public int maxPlayers = 4;

    private NetworkVariable<ScoreTable> scoreTable =
        new NetworkVariable<ScoreTable>(
            new ScoreTable(GameManager.MAX_PLAYERS),
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

    void Awake()
    {

        Debug.Log("instance is null: " + UnityEngine.Object.ReferenceEquals(instance, null));
        maxPlayers = GameManager.MAX_PLAYERS;
        if (UnityEngine.Object.ReferenceEquals(instance, null))
        {
            instance = this;
            GetComponent<NetworkObject>().Spawn();
            DontDestroyOnLoad(gameObject); // Make sure That GameObject won't be Destroyed by loading in Other Scenes.
            Initialize();
        }
        else
        {
            Destroy(gameObject); // If instance already exits, destroy the gameobject
        }
    }

    void Start()
    {

    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Debug.Log("OnNetworkSpawn");
        if (!IsHost) return;

        List<PlayerNetwork> players = GameManager.instance.getConnectedPlayers();
        Debug.Log("Players: " + players.Count);
        foreach (PlayerNetwork p in players)
        {

            Health health = p.GetComponent<Health>();
            health.registerOnDeathCallback(AddOnePoint);
            Debug.Log("Subscribed to health of: " + p.GetComponent<IDHolder>().getClientID());
        }

    }

    public void Initialize()
    {
    }

    /*
    public static PointManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("PointManager");
                instance = obj.AddComponent<PointManager>();
                instance.Initialize();
                DontDestroyOnLoad(obj); // Make sure That GameObject won't be Destroyed by loading in Other Scenes.
            }
            return instance;
        }
    }
*/
    public void AddOnePoint(ulong playerIndex)
    {

        AddPoints((int)playerIndex, 1);
    }

    public void AddPoints(int playerIndex, int amount)
    {

        if (GameManager.instance.isClientStillConnected(playerIndex))
        {

            scoreTable.Value.addPoints(playerIndex, amount);
        }
    }

    public void ReducePoints(int playerIndex, int amount)
    {

        if (GameManager.instance.IsPlayerIndexConnected(playerIndex))
        {

            scoreTable.Value.reducePoints(playerIndex, amount);
        }
    }

    public void SetPoints(int playerIndex, int amount)
    {

        if (GameManager.instance.IsPlayerIndexConnected(playerIndex))
        {

            scoreTable.Value.setPoints(playerIndex, amount);
        }
    }

    public int getPoints(int playerIndex)
    {

        if (GameManager.instance.IsPlayerIndexConnected(playerIndex))
        {

            return scoreTable.Value.getPointsOfPlayer(playerIndex);
        }

        return -1;

        // Never throw an exception
        // if one player disconnets the game might crash for everyone
        //throw new System.Exception("Points: Index Number is outside of Range");
    }

    public int[] getScoreTable()
    {

        return scoreTable.Value.asArray();
    }

}