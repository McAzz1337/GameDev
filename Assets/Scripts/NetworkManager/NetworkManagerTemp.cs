using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class NetworkManagerTemp : MonoBehaviour
{

    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private TextMeshProUGUI readyText;

    void Start()
    {

        switchToConnectionChoice();

        hostBtn.onClick.AddListener(onStartHost);
        clientBtn?.onClick.AddListener(onStartClient);
    }

    public void onStartHost()
    {
        switchToReadyUp();
        LobbyManager.instance.init(NetworkManager.Singleton.LocalClientId);
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("CharacterScene", LoadSceneMode.Single);
    }

    public void onStartClient()
    {
        if (clientBtn == null) return;

        switchToReadyUp();
        LobbyManager.instance.init(NetworkManager.Singleton.LocalClientId);

        NetworkManager.Singleton.StartClient();
    }


    private void switchToReadyUp()
    {

        hostBtn.gameObject.SetActive(false);
        clientBtn?.gameObject.SetActive(false);
    }

    private void switchToConnectionChoice()
    {

        hostBtn.gameObject.SetActive(true);
        clientBtn?.gameObject.SetActive(true);
    }
}
