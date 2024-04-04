using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class GameManager : NetworkBehaviour
{

    public static GameManager instance;
    [SerializeField] private List<PlayerNetwork> connectedPlayers;
    private NetworkList<PlayerData> playerDataNetworkList;

    public static int MAX_PLAYERS = 4;

    public event EventHandler OnPlayerDataNetworkListChanged;
    public event EventHandler OnReadyChanged;


    [SerializeField]
    private NetworkVariable<ReadyState> ready =
                            new NetworkVariable<ReadyState>(
                                new ReadyState(MAX_PLAYERS),
                                NetworkVariableReadPermission.Owner,
                                NetworkVariableWritePermission.Server
                            );

    [SerializeField] private Canvas uiCanvas;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback += clientConnected;
        NetworkManager.Singleton.OnClientConnectedCallback += addToNetworkPlayerList;

        NetworkManager.Singleton.OnClientDisconnectCallback += clientDisconnected;
    }

    void Awake()
    {

        instance = this;
        DontDestroyOnLoad(gameObject);
        connectedPlayers = new List<PlayerNetwork>();
        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += OnPlayerDataNetwork_OnListChanged;
    }

    private void OnPlayerDataNetwork_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }



    private void addToNetworkPlayerList(ulong clientId)
    {
        if (!IsHost) return;

        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,
        });
    }


    public void clientConnected(ulong clientID)
    {

        if (!IsHost) return;

        PlayerNetwork player =
                    NetworkManager.Singleton.ConnectedClients[clientID]
                    .PlayerObject.GetComponent<PlayerNetwork>();

        connectedPlayers.Add(player);
    }

    public void clientDisconnected(ulong clientID)
    {

        if (!IsHost) return;

        foreach (PlayerNetwork player in connectedPlayers)
        {

            IDHolder holder = player.GetComponent<IDHolder>();

            if (holder.getClientID() == clientID)
            {

                connectedPlayers.Remove(player);
                ready.Value.unreadyPlayer((int)clientID);
                break;
            }
        }

        allPlayersReadyCheck();
    }

    public void allPlayersReadyCheck()
    {


        if (playerDataNetworkList.Count > 0 && ready.Value.allReady(connectedPlayers.Count))
        {
            PointManager.Instance.maxPlayers = GameManager.MAX_PLAYERS;
            Debug.Log("Everybody is ready");
            MapLoader.LoadRandomSceneFromFolder();
            //MapLoader.loadMap("002");
            //NetworkManager.Singleton.SceneManager.LoadScene("Map_001", LoadSceneMode.Single);
        }
    }





    public bool IsPlayerReady(ulong clientID)
    {

        return ready.Value.IsReady((int)clientID);
    }



    public void readyPlayer(ulong clientID)
    {
        readyPlayerServerRpc(clientID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void readyPlayerServerRpc(ulong clientID)
    {
        ready.Value.readyPlayer((int)clientID);
        readyPlayerClientRpc(clientID);

        allPlayersReadyCheck();
    }

    [ClientRpc]
    public void readyPlayerClientRpc(ulong clientID)
    {
        ready.Value.readyPlayer((int)clientID);
        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }

    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }

    public int getPlayerCount()
    {

        return connectedPlayers.Count;
    }

    public List<PlayerNetwork> getConnectedPlayers()
    {

        return connectedPlayers;
    }

    public bool isClientStillConnected(int index)
    {

        for (int i = 0; i < connectedPlayers.Count; i++)
        {
            if ((ulong)index == connectedPlayers[i].GetComponent<IDHolder>().getClientID())
            {

                return true;
            }
        }

        return false;
    }

}
