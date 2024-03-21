using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    private static PointManager instance;

    // Class to Save Points of one Player.
    private class PlayerPoints
    {
        public int points = 0;
        public Text pointsText;
    }

    public int maxPlayers = 2;
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
            playerPointsList[i] = new PlayerPoints();
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

    public void AddPoints(int playerIndex, int amount)
    {
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
        if(playerIndex >= 0 && playerIndex < maxPlayers)
        {
            return playerPointsList[playerIndex].points;
        }
        throw new System.Exception("Points: Index Number is outside of Range");
    }

    // Update UI Text for a certain Player
    private void UpdatePointsText(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            if (playerPointsList[playerIndex].pointsText != null)
            {
                playerPointsList[playerIndex].pointsText.text = "Player " + (playerIndex + 1) + " Points: " + playerPointsList[playerIndex].points.ToString();
            }
        }
    }

    public void SetPointsText(int playerIndex, Text pointsText)
    {
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            playerPointsList[playerIndex].pointsText = pointsText;
            UpdatePointsText(playerIndex);
        }
    }

    public Text getPointText(int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < maxPlayers)
        {
            return playerPointsList[playerIndex].pointsText;
        }
        throw new System.Exception("PointText: Index Number is outside of Range");

    }
}