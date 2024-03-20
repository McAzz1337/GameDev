using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSceneUI : NetworkBehaviour
{
    [SerializeField] private Button readyButton;

    private void Awake()
    {
        readyButton.onClick.AddListener(() =>
        {
            GameManager.instance.readyPlayer(NetworkManager.Singleton.LocalClientId);
        });

    }
}
