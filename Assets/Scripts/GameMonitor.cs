using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameMonitor : MonoBehaviour
{
    [SerializeField] public int winningConditionScore = 10;
    private bool isPlaying = true;
    public static int sceneIndex;
    public PointManager pointManager;
    private List<PlayerScore> scorelist = new List<PlayerScore>();

    private void Awake()
    {
        loadScoreList();
        PointManager.Instance.maxPlayers = transform.childCount;
        pointManager = PointManager.Instance;
    }

    public void Start()
    {
        isPlaying = true;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        //loadScoreList();
        Debug.Log("Amount Players: " + pointManager.maxPlayers);
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
                else
                {
                    Debug.Log("Called");
                    SceneManager.LoadSceneAsync(3);
                }
            }
        }
    }

    private void loadScoreList()
    {
        this.scorelist = new List<PlayerScore>();
        // Gehe durch jedes Kind des GameObjects
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                // Hole das Transform des aktuellen Kindes
                GameObject child = transform.GetChild(i).gameObject;

                PlayerScore p = new PlayerScore(child);
                this.scorelist.Add(p);
                Debug.Log("score: " + p.getScore());
            }
        }
        else
        {
            Debug.LogWarning("No TargetObject Available");
        }
    }
    private void loadWinningScene(int winningPlayerIndex)
    {
        PlayerPrefs.SetInt("WinningPlayer", winningPlayerIndex);
        SceneManager.LoadScene("WinningScene");
    }

    private bool isRoundConcluded()
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
        Debug.Log("Amount Players: " + remainingPlayers);

        return remainingPlayers <= 1;
    }


    private void scoringByLastSurvivor()
    {
        for(int i = 0; i < this.scorelist.Count; i++)
        {
            TargetEventChecker checker = this.scorelist[i].getTargetEventChecker();
            if (checker.getIsDeath() == false)
            {
                pointManager.AddPoints(i, 1);
            }

        }
    }

    private int getWinningPlayer()
    {
        for(int i = 0;i < this.scorelist.Count; i++)
        {
            if (pointManager.getPoints(i) == winningConditionScore) {
                return i;
            }
        }    
        return -1;

    }

    public void getCurrentScores()
    {
        for (int i = 0; i < this.scorelist.Count ; i++) {
            Debug.Log("Player " + i + " : " + pointManager.getPoints(i));

        }
    }
}
