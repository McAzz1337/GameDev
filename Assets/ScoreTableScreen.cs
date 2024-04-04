using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreTableScreen : NetworkBehaviour
{
    public TextMeshProUGUI scoreText;
    public PointManager pointManager;

    // Start is called before the first frame update

    private void Awake()
    {
        pointManager = PointManager.Instance;
    }

    void Start()
    {
        loadScoreList();
    }


    public void RestartRound()
    {
        MapLoader.LoadRandomSceneFromFolder();
        //SceneManager.LoadScene(ScoreManager.sceneIndex);
    }


    private void loadScoreList()
    {
        Debug.Log(pointManager.maxPlayers);
        scoreText.text = "";
        for (int i = 0; i < pointManager.maxPlayers; i++) {
            TextMeshProUGUI playerText = pointManager.getPointText(i);

            if (playerText != null)
            {
                // Füge den Text des Spielers zum scoreText hinzu
                scoreText.text += playerText.text + Environment.NewLine;
            }
            else
            {
                // Wenn der Text null ist, füge eine Nachricht hinzu, dass der Spieler nicht existiert
                scoreText.text += "Player " + (i + 1) + " Points: Player does not exist" + Environment.NewLine;
            }
        }
    }
}
