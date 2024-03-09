using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerTemp : MonoBehaviour
{

    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button readyBtn;
    [SerializeField] private TextMeshProUGUI readyText;

    void Start()
    {

        switchToConnectionChoice();

        hostBtn.onClick.AddListener(onStartHost);
        clientBtn.onClick.AddListener(onStartClient);
        readyBtn.onClick.AddListener(onReadyPressed);
    }

    public void onReadyPressed()
    {

        GameManager.instance.readyPlayer(NetworkManager.Singleton.LocalClientId);

        readyText.gameObject.SetActive(true);
    }

    public void onStartHost()
    {

        switchToReadyUp();

        NetworkManager.Singleton.StartHost();
    }

    public void onStartClient()
    {

        switchToReadyUp();

        NetworkManager.Singleton.StartClient();
    }


    private void switchToReadyUp()
    {

        hostBtn.gameObject.SetActive(false);
        clientBtn.gameObject.SetActive(false);
        readyBtn.gameObject.SetActive(true);
        readyText.gameObject.SetActive(false);
    }

    private void switchToConnectionChoice()
    {

        hostBtn.gameObject.SetActive(true);
        clientBtn.gameObject.SetActive(true);
        readyBtn.gameObject.SetActive(false);
        readyText.gameObject.SetActive(false);
    }
}
