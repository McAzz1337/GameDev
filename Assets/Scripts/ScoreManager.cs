using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] public int winningConditionScore = 10;
    private List<PlayerScore> scorelist;
    private bool isPlaying = true;

    public void Start()
    {

        this.scorelist = new List<PlayerScore>();
        // Gehe durch jedes Kind des GameObjects
        for (int i = 0; i < transform.childCount; i++)
        {
            // Hole das Transform des aktuellen Kindes
            GameObject child = transform.GetChild(i).gameObject;

            this.scorelist.Add(new PlayerScore(child));
        }

        Debug.Log("Amount Players: " + scorelist.Count);

    }

    public void Update()
    {
        if (isPlaying)
        {
            if (isRoundConcluded())
            {
                isPlaying = false;
                scoringByLastSurvivor();
                getCurrentScores();
                int winningPlayer = getWinningPlayer();
                if (winningPlayer != -1)
                {
                    Debug.Log("Winner: Player" + winningPlayer);
                    loadWinningScene(winningPlayer);
                }
            }
        }
        
    }

    public bool isRoundConcluded()
    {
        int remainingPlayers = this.scorelist.Count;
        foreach (PlayerScore p in scorelist)
        {
            TargetEventChecker checker = p.getPlayer().GetComponent<TargetEventChecker>();
            if (checker.getIsDeath())
            {
                remainingPlayers--;
            }
        }

        return remainingPlayers <= 1;
    }


    public void scoringByLastSurvivor()
    {
        foreach (PlayerScore score in scorelist)
        {
            TargetEventChecker checker = score.getPlayer().GetComponent<TargetEventChecker>();
            if (checker.getIsDeath() == false)
            {
                score.addScore(1);
            }

        }
    }

    public int getWinningPlayer()
    {
        int playerindex = 0;
        foreach (PlayerScore score in scorelist)
        {
          if(score.getScore() == winningConditionScore)
            {
                return playerindex;
            }
            playerindex++;
        }
        return -1;

    }

    public void getCurrentScores()
    {
        int playerindex = 0;
        foreach (PlayerScore score in scorelist)
        {
            Debug.Log("Winner: Player " + playerindex + " : " + score.getScore());
            playerindex++;

        }
    }

    void loadWinningScene(int winningPlayerIndex)
    {
        PlayerPrefs.SetInt("WinningPlayer", winningPlayerIndex);
        SceneManager.LoadScene("WinningScreen");
    }
}
