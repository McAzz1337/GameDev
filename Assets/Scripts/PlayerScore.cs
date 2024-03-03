using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public GameObject player;
    public int score = 0;


    public PlayerScore(GameObject player)
    {
        this.player = player;
    }

    // Methode zum Hinzufügen von Punkten für Spieler 1
    public void addScore(int points)
    {
        score += points;
    }

    // Methode zum Hinzufügen von Punkten für Spieler 2
    public void reduceScore(int points)
    {
        score -= points;
    }

    public GameObject getPlayer()
    {
        return this.player;
    }

    public int getScore()
    {
        return score;
    }
}