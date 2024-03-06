using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{    
    [SerializeField] public int winningConditionScore = 10;
    private static ScoreManager instance;
    private List<PlayerScore> scorelist;
    private bool isPlaying = true;
    public static int sceneIndex;


    public static ScoreManager Instance
    {
        get
        {
            // Wenn keine Instanz vorhanden ist, erstelle eine neue
            if (instance == null)
            {
                GameObject obj = new GameObject("ScoreManager");
                instance = obj.AddComponent<ScoreManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    public void Start()
    {
        isPlaying = true;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadScoreList();
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
                saveScorelist();
                int winningPlayer = getWinningPlayer();
                
                if (winningPlayer != -1)
                {
                    Debug.Log("Winner: Player" + winningPlayer);

                    //SceneManager.LoadSceneAsync("WinningScreen");
                }  else
                {
                    SceneManager.LoadSceneAsync("ScoreTable");
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
        Debug.Log("Amount Players: " + remainingPlayers);

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
            Debug.Log("Player " + playerindex + " : " + score.getScore());
            playerindex++;

        }
    }


    private void saveScorelist()
    {
        string json = JsonUtility.ToJson(scorelist);
        PlayerPrefs.SetString("scorelist", json);
        PlayerPrefs.Save();
    }

    private void loadScoreList()
    {
        string json = PlayerPrefs.GetString("scorelist");
        //Debug.Log("JSON: "+ json);
        if (json != "{}") //!string.IsNullOrEmpty(json)
        {
            //Debug.Log("i Fall here");
            this.scorelist = JsonUtility.FromJson<List<PlayerScore>>(json);
        } 
        else
        {
            //Debug.Log("Correct Fall");
            this.scorelist = new List<PlayerScore>();
            // Gehe durch jedes Kind des GameObjects
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    // Hole das Transform des aktuellen Kindes
                    GameObject child = transform.GetChild(i).gameObject;

                    this.scorelist.Add(new PlayerScore(child));
                }
            } else
            {
                Debug.LogWarning("No TargetObject Available");
            }
        }
    }



    void loadWinningScene(int winningPlayerIndex)
    {
        PlayerPrefs.SetInt("WinningPlayer", winningPlayerIndex);
        SceneManager.LoadScene("WinningScene");
    }

}
