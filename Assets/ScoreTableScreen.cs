using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreTableScreen : MonoBehaviour
{
    private List<PlayerScore> scorelist;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
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
        string json = PlayerPrefs.GetString("scorelist");
        Debug.Log(json);
        this.scorelist = JsonUtility.FromJson<List<PlayerScore>>(json);
        scoreText.text = "";
        int playerindex = 1;
        foreach (PlayerScore score in scorelist)
        {
            scoreText.text += "Player " + playerindex + " : " + score.getScore() + Environment.NewLine;
            playerindex++;
        }
        
    }
}
