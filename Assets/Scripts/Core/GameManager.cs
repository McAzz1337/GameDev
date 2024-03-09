using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class GameManager : NetworkBehaviour
{

    [SerializeField] private GameObject[] toActivate;
    public static GameManager instance;
    [SerializeField] private Transform[] spawnTransforms;
    [SerializeField] private PlayerNetwork[] connectedPlayers;

    private const int MAX_PLAYERS = 4;

    private int playerCount = 0;


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
    }

    void Awake()
    {

        instance = this;
        connectedPlayers = new PlayerNetwork[spawnTransforms.Length];

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        allPlayersReadyCheck();
        ready.Value.log();
    }


    public void clientConnected(ulong clientID)
    {

        if (!IsHost) return;

        Debug.Log("Connected player: " + clientID);
        for (int i = 0; i < spawnTransforms.Length; i++)
        {

            if (connectedPlayers[i] == null)
            {

                connectedPlayers[i] =
                    NetworkManager.Singleton.ConnectedClients[clientID]
                    .PlayerObject.GetComponent<PlayerNetwork>();

                connectedPlayers[i].transform.position = spawnTransforms[i].position;
                playerCount++;

                break;
            }
        }
    }

    public void allPlayersReadyCheck()
    {

        if (!IsHost) return;


        if (ready.Value.allReady(playerCount))
        {

            startGameClientRpc();
        }

    }

    [ClientRpc]
    public void startGameClientRpc()
    {

        Debug.Log("Start Game");
        acitvate();
        uiCanvas.enabled = false;
    }

    public void acitvate()
    {

        foreach (GameObject g in toActivate)
        {

            g.SetActive(true);
        }
    }

    public void readyPlayer(ulong clientID)
    {


        readyPlayerServerRpc(clientID);
    }

    [ServerRpc(RequireOwnership = false)]
    public void readyPlayerServerRpc(ulong clientID)
    {

        Debug.Log("readyPlayer: " + clientID);
        ready.Value.readyPlayer((int)clientID);
    }


}
