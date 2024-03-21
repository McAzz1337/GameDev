using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreTableScreen : MonoBehaviour
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
        SceneManager.LoadScene(ScoreManager.sceneIndex);
    }


    private void loadScoreList()
    {
        for(int i = 0; i < pointManager.maxPlayers; i++) {
            scoreText.text = pointManager.getPointText(i) + Environment.NewLine;
        }
    }
}
