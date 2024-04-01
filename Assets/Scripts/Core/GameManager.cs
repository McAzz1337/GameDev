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
    [SerializeField] private PlayerNetwork[] connectedPlayers;
    private NetworkList<PlayerData> playerDataNetworkList;

    public static int MAX_PLAYERS = 4;

    bool checkIfReady = true;

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
        connectedPlayers = new PlayerNetwork[MAX_PLAYERS];
        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += OnPlayerDataNetwork_OnListChanged;
    }

    private void OnPlayerDataNetwork_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    void Start()
    {

    }

    void Update()
    {

        if (!IsHost) return;

        allPlayersReadyCheck();
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

        for (int i = 0; i < MAX_PLAYERS; i++)
        {

            if (connectedPlayers[i] == null)
            {

                connectedPlayers[i] =
                    NetworkManager.Singleton.ConnectedClients[clientID]
                    .PlayerObject.GetComponent<PlayerNetwork>();

                //connectedPlayers[i].transform.position = spawnTransforms[i].position;

                break;
            }
        }
    }

    public void clientDisconnected(ulong clientID)
    {

        if (!IsHost) return;

        connectedPlayers[clientID] = null;
        Debug.Log("Client disconnected: " + clientID);

        Debug.Log("Count: " + NetworkManager.Singleton.ConnectedClients.Count);
        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {

            if (connectedPlayers[i] == null) continue;

            IDHolder holder = NetworkManager.Singleton.ConnectedClients[(ulong)i]
                                .PlayerObject.GetComponent<IDHolder>();

            Debug.Log("Connected client[" + i + "] has id: " + holder.getClientID());
        }
    }

    public void allPlayersReadyCheck()
    {

        if (!IsHost || !checkIfReady) return;


        if (playerDataNetworkList.Count > 0 && ready.Value.allReady(playerDataNetworkList.Count))
        {
            Debug.Log("Everybody is ready");
            NetworkManager.Singleton.SceneManager.LoadScene("Map_004", LoadSceneMode.Single);
            checkIfReady = false;
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

        return NetworkManager.Singleton.ConnectedClients.Count;
    }

    public bool isClientStillConnected(int index)
    {

        return connectedPlayers[index] != null;
    }

}
