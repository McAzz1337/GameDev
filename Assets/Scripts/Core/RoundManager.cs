using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{



    void Start()
    {
        if (!IsHost) return;

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {

            NetworkClient client = NetworkManager.Singleton.ConnectedClients[(ulong)i];
            PlayerNetwork player = client.PlayerObject.GetComponent<PlayerNetwork>();
            player.enableBattleControls();

            PlayerSpawner.instance.SpawnPlayer(player);
        }
    }

    public void endRound()
    {

        for (int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
        {

            NetworkClient client = NetworkManager.Singleton.ConnectedClients[(ulong)i];
            PlayerNetwork player = client.PlayerObject.GetComponent<PlayerNetwork>();
            player.disableBattleControls();
        }
    }


}
