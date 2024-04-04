using TMPro;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static Health;

public class PointManager : NetworkBehaviour
{
    private static PointManager instance;

    // Class to Save Points of one Player.
    private class PlayerPoints
    {
        public int points = 0;
        public String pointsText;

        public PlayerPoints(int playerIndex)
        {
            this.pointsText = "Player " + (playerIndex + 1) + " Points: 0";
        }


    }

    public int maxPlayers = 4;
    private PlayerPoints[] playerPointsList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make sure That GameObject won't be Destroyed by loading in Other Scenes.
            Initialize();
            
        }
        else
        {
            Destroy(gameObject); // If instance already exits, destroy the gameobject
        }
    }

    public void Initialize()
    {
        // Create a List of PlayerPoints
        playerPointsList = new PlayerPoints[maxPlayers];
        for (int i = 0; i < maxPlayers; i++)
        {
            playerPointsList[i] = new PlayerPoints(i);
        }
    }

    public static PointManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("PointManager");
                instance = obj.AddComponent<PointManager>();
                DontDestroyOnLoad(obj); // Make sure That GameObject won't be Destroyed by loading in Other Scenes.
            }
            return instance;
        }
    }

    public void AddOnePoint(int playerIndex)
    {
        AddPoints(playerIndex, 1);
    }

    public void AddPoints(int playerIndex, int amount)
    {
        Debug.Log("Add Point for Player: " + playerIndex);
        Debug.Log("playerPointsList :" + playerPointsList.Length);
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            playerPointsList[playerIndex].points += amount;
            UpdatePointsText(playerIndex);
        }
    }

    public void ReducePoints(int playerIndex, int amount)
    {
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            playerPointsList[playerIndex].points -= amount;
            UpdatePointsText(playerIndex);
        }
    }

    public void SetPoints(int playerIndex, int amount)
    {
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            playerPointsList[playerIndex].points = amount;
            UpdatePointsText(playerIndex);
        }
    }

    public int getPoints(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            return playerPointsList[playerIndex].points;
        }
        throw new System.Exception("Points: Index Number is outside of Range");
    }

    // Update UI Text for a certain Player
    private void UpdatePointsText(int playerIndex)
    {
        Debug.Log("Enter Methode Update Text: " + playerIndex);
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            if (playerPointsList[playerIndex].pointsText != null)
            {
                Debug.Log("Update Text: " + playerIndex);
                playerPointsList[playerIndex].pointsText = "Player " + (playerIndex + 1) + " Points: " + playerPointsList[playerIndex].points.ToString();
            }
        }
    }

    public void SetPointsText(int playerIndex, String pointsText)
    {
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            playerPointsList[playerIndex].pointsText = pointsText;
            UpdatePointsText(playerIndex);
        }
    }

    public String getPointText(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            return playerPointsList[playerIndex].pointsText;
        }
        throw new System.Exception("PointText: Index Number is outside of Range");

    }
}