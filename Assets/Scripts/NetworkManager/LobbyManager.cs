using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{

    public static LobbyManager instance = null;
    private Lobby hostLobby = null;
    private float heartBeatTimer = 20.0f;

    void Awake()
    {

        Debug.Log("awake");
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {

        instance = null;
    }

    void Update()
    {

        sendHearBeat();
    }

    private async void sendHearBeat()
    {

        if (hostLobby == null) return;


        heartBeatTimer -= Time.deltaTime;

        if (heartBeatTimer < 0.0f)
        {

            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);

            heartBeatTimer = 20.0f;
        }
    }

    public async void init(bool create)
    {

        Debug.Log("init");
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += onSignIn;

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        if (create)
        {

            createLobby();
        }
        else
        {

            joinLobby();
        }
    }

    private void onSignIn()
    {

        Debug.Log("Logged in: " + AuthenticationService.Instance.PlayerId);
        while (LobbyInfo.instance == null)
        {

        }
        LobbyInfo.instance.setInfo("Logged in: " + AuthenticationService.Instance.PlayerId);
    }

    public async void createLobby()
    {

        try
        {
            string name = "MyLobby";

            hostLobby = await LobbyService.Instance.CreateLobbyAsync(name, GameManager.MAX_PLAYERS);
            if (hostLobby != null)
            {
                LobbyInfo.instance.setText(hostLobby.Id.ToString());
            }
            else
            {

                LobbyInfo.instance.setText("no lobby created");
            }

            Debug.Log("Created Lobby:" + name);
        }
        catch (LobbyServiceException e)
        {

            Debug.Log("createLobby: " + e);
        }

        listLobbies();
    }

    public async void listLobbies()
    {

        try
        {
            QueryResponse response = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found:" + response.Results.Count);
            foreach (Lobby l in response.Results)
            {

                Debug.Log(l.Name + ": " + l.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {

            Debug.Log("listLobbies: " + e);
        }
    }


    public async void joinLobby()
    {

        listLobbies();
        try
        {
            QueryResponse response = await Lobbies.Instance.QueryLobbiesAsync();
            string s = "a";
            foreach (Lobby l in response.Results)
            {

                s += "name: " + l.Name + "\tid: " + l.Id + "\n";
            }
            LobbyInfo.instance.setText(s);

            if (response.Results.Count > 0)
            {
                Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(response.Results[0].Id);
            }


        }
        catch (LobbyServiceException e)
        {

            LobbyInfo.instance.setText(e.Message);
        }
    }
}
