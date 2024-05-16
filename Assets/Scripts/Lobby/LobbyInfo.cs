using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Authors: Marc Federspiel
public class LobbyInfo : MonoBehaviour
{

    public static LobbyInfo instance = null;
    [SerializeField] private TextMeshProUGUI lobbyID;
    [SerializeField] private TextMeshProUGUI info;

    void Awake()
    {

        instance = this;
    }
    void Start()
    {

        DontDestroyOnLoad(gameObject);

    }

    public void setText(string text)
    {

        if (text == null)
        {

            text = "none";
        }

        lobbyID.text = "Lobby: " + text;
    }

    public void setInfo(string text)
    {

        info.text = text;
    }
}
