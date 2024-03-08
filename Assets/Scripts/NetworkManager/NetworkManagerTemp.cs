using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerTemp : MonoBehaviour
{

    [SerializeField] private Canvas canvas;

    public void onStartHost()
    {

        canvas.enabled = false;
        NetworkManager.Singleton.StartHost();
        GameManager.instance.acitvate();
    }

    public void onStartClient()
    {

        canvas.enabled = false;
        NetworkManager.Singleton.StartClient();
    }
}
